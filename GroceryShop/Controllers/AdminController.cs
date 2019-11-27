using GroceryShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroceryShop.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
       
        private GSDBContext db = new GSDBContext();

        public ActionResult DashBoard()
        {
            ViewBag.OrderCount = db.Orders.Count();
            ViewBag.ProductCount = db.Products.Count();
            ViewBag.CategoryCount = db.Categories.Count();
            ViewBag.MessageCount = db.Messages.Count();
            return View();
        }
    }
}