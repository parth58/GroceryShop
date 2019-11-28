﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GroceryShop.Models;
using GroceryShop.ViewModels;

namespace GroceryShop.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ProductsController : Controller
    {
        private GSDBContext db = new GSDBContext();

        // GET: Products
        public ActionResult Index(string search)
        {
            var products = db.Products.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name != null && p.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(products);
        }
        public ActionResult ProductTable(string search)
        {
                var products = db.Products.ToList();
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p =>p.Name!=null && p.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(products);
        }

        // GET: Products/Details/5
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

        // GET: Products/Create
        public ActionResult Create()
        {
           
            NewProductViewModel model = new NewProductViewModel();

            model.AvailableCategories = db.Categories.ToList();
            return View(model);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create(NewProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new Product();
                newProduct.Name = model.Name;
                newProduct.Description = model.Description;
                newProduct.OriginalPrice = model.OriginalPrice;
                newProduct.SellingPrice = model.SellingPrice;
                newProduct.stock = model.stock;
                newProduct.Tags = model.Tags;
                newProduct.Category = db.Categories.Find(model.CategoryID);
             
                newProduct.ImageURL = model.ImageURL;
                newProduct.CreatedOn = DateTime.Now;
                newProduct.UpdatedOn = DateTime.Now;
                db.Products.Add(newProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            EditProductViewModel model = new EditProductViewModel();

            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            model.ID = product.ID;
            model.Name = product.Name;
            model.Description = product.Description;
            model.OriginalPrice = product.OriginalPrice;
            model.SellingPrice = product.SellingPrice;
            model.stock = product.stock;
            model.CategoryID = product.Category != null ? product.Category.ID : 0;
            model.ImageURL = product.ImageURL;
            model.Tags = product.Tags;

            model.AvailableCategories = db.Categories.ToList();

            return View(model);

        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = db.Products.Find(model.ID);
                existingProduct.Name = model.Name;
                existingProduct.Description = model.Description;
                existingProduct.OriginalPrice = model.OriginalPrice;
                existingProduct.SellingPrice = model.SellingPrice;
                existingProduct.stock = model.stock;
                existingProduct.Category = null; //mark it null. Because the referncy key is changed below
                existingProduct.CategoryID = model.CategoryID;
                //existingProduct.Category = db.Categories.Find(model.CategoryID);
                existingProduct.Tags = model.Tags;
                existingProduct.UpdatedOn = DateTime.Now;
                //don't update imageURL if its empty
                if (!string.IsNullOrEmpty(model.ImageURL))
                {
                    existingProduct.ImageURL = model.ImageURL;
                }
                db.Entry(existingProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }
         
           
            return View(model);

        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
