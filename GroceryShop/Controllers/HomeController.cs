using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GroceryShop.Models;

using GroceryShop.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;

namespace GroceryShop.Controllers
{
   

    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        private GSDBContext db = new GSDBContext();
        public ActionResult Index(string search, int? categoryID=null, int? sortBy = 0, int? minimumPrice=0, int? maximumPrice=null, int? page=null)
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.MinimumPrice = 0;
            if (minimumPrice.HasValue)
            {
                ViewBag.MinimumPrice = minimumPrice;
            }
            ViewBag.MaximumPrice = (int)(db.Products.Max(x => x.SellingPrice));
            if (maximumPrice.HasValue)
            {
                ViewBag.MaximumPrice = maximumPrice;
            }
            ShopViewModel model = new ShopViewModel();

            model.Search = search;
            model.CategoryID = categoryID;
            model.MinimumPrice = ViewBag.MinimumPrice;
            model.MaximumPrice = (int)(db.Products.Max(x => x.SellingPrice));
            model.SortBy = sortBy;
            return View(model);
        }
      
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Products(string search, int? categoryID, int? page, int? minimumPrice, int? maximumPrice, int? sortBy)
        {

            ViewBag.Search = search;
            ViewBag.CategoryID = categoryID;
            ViewBag.SortBy = sortBy;
             
            var products = db.Products.ToList();

            if (categoryID.HasValue)
            {
                products = products.Where(x => x.Category.ID == categoryID.Value).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Category.Name.ToLower().Contains(search.ToLower()) || p.Name.ToLower().Contains(search.ToLower())).ToList();
            }

            if (minimumPrice.HasValue)
            {
                products = products.Where(x => x.SellingPrice >= minimumPrice.Value).ToList();
            }

            if (maximumPrice.HasValue)
            {
                products = products.Where(x => x.SellingPrice <= maximumPrice.Value).ToList();
            }

            if (sortBy.HasValue)
            {
                switch (sortBy.Value)
                {
                    case 2:
                        products = products.OrderByDescending(x => x.ID).ToList();
                        break;
                    case 3:
                        products = products.OrderBy(x => x.SellingPrice).ToList();
                        break;
                    case 4:
                        products = products.OrderByDescending(x => x.SellingPrice).ToList();
                        break;
                    default:
                        break;
                }
            }
            IPagedList<Product> pr = products.ToPagedList(page ?? 1, 12);
               

            return PartialView(pr);
        }
        public ActionResult Cart()
        {
            
            return View();
        }
       
        [Authorize]
        public ActionResult CheckOut()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.User = user;
            return View();
        }
        [Authorize]
        public ActionResult MyOrders(string userId,string status)
        {
            var orders= db.Orders.Where(x => x.UserID == userId).Include(x => x.OrderItems);
            OrderViewModel model = new OrderViewModel();
            model.PendingOrders= orders.Where(x => x.Status.ToLower().Contains("pending")).OrderByDescending(x => x.Datetime).ToList();
            model.InProgressOrders = orders.Where(x => x.Status.ToLower().Contains("inprogress")).OrderByDescending(x => x.Datetime).ToList();
            model.PreviousOrders = orders.Where(x => x.Status.ToLower().Contains("delivered")).OrderByDescending(x => x.Datetime).ToList();

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Error404()
        {
            return View();
        }


        public JsonResult PlaceOrder(Order model)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            Order newOrder = new Order();
            newOrder.UserID = User.Identity.GetUserId();
            newOrder.Datetime = DateTime.Now;
            newOrder.Status = "Pending";
            newOrder.FirstName = model.FirstName;
            newOrder.LastName = model.LastName;
            newOrder.PhoneNumber = model.PhoneNumber;
            newOrder.Address1 = model.Address1;
            newOrder.Address2 = model.Address2;
            newOrder.City = model.City;
            newOrder.ZipCode = model.ZipCode;
            newOrder.TotalAmount = model.TotalAmount;
            newOrder.PaymentType = model.PaymentType;
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                List<OrderItem> orderitems = new List<OrderItem>();
                foreach (var item in cart)
                {
                   
                    if (item.Quantity >0)
                    {
                        OrderItem orderItem = new OrderItem();
                        orderItem.Quantity = item.Quantity;
                        orderItem.ProductID = item.Product.ID;
                        orderitems.Add(orderItem);
                       
                    }
                }

                newOrder.OrderItems = orderitems;

                db.Orders.Add(newOrder);
                db.SaveChanges();
                Session["cart"] = null;
                result.Data = new { Success = true, Message = "Product Updated to cart successfully"};

            }

            return result;
        }
    }
}