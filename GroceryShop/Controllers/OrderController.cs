using GroceryShop.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GroceryShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private GSDBContext db = new GSDBContext();
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

        // GET: Order

        public ActionResult Index(String status)
        {
            var orders = db.Orders.ToList();
            if (!string.IsNullOrEmpty(status))
            {
              orders = orders.Where(x => x.Status.ToLower().Contains(status.ToLower())).ToList();
            }
            return View(orders);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.AvailableStatuses = new List<string>() { "Pending", "InProgress", "Delivered" };

            return View(order);
        }
        public JsonResult ChangeStatus(string status, int ID)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            
            var order = db.Orders.Find(ID);

            order.Status = status;

            db.Entry(order).State = EntityState.Modified;

           
            result.Data = new { Success = db.SaveChanges()>0 };

            return result;
        }
    }
}