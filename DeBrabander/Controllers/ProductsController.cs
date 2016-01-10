﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeBrabander.DAL;
using DeBrabander.Models;
using DeBrabander.ViewModels.Products;

namespace DeBrabander.Controllers
{
    public class ProductsController : Controller
    {
        private Context db = new Context();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
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
            int CId = product.CategoryId;
            Category category = db.Categories.Find(CId);
            int VId = product.VATPercId;
            VAT vat = db.VATs.Find(VId);

            ProductDetailsViewModel pdvm = new ProductDetailsViewModel();
            pdvm.ProductName = product.ProductName;
            pdvm.ProductCode = product.ProductCode;
            pdvm.Remark = product.Remark;
            pdvm.Description = product.Description;
            pdvm.PriceExVAT = product.PriceExVAT;
            pdvm.Reprobel = product.Reprobel;
            pdvm.Bebat = product.Bebat;
            pdvm.Recupel = product.Recupel;
            pdvm.Auvibel = product.Auvibel;
            pdvm.PurchasePrice = product.PurchasePrice;
            pdvm.Brand = product.Brand;
            pdvm.CategoryName = category.CategoryName;
            pdvm.VATValue = vat.VATValue;
            pdvm.Stock = product.Stock;
            pdvm.EAN = product.EAN;
            pdvm.StockControl = product.StockControl;

            return View(pdvm);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductCode,Remark,Description,PriceExVAT,Reprobel,bebat,Recupel,Auvibel,PurchasePrice,Brand,CategoryId,VATPercId,Stock,EAN")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductCode,Remark,Description,PriceExVAT,Reprobel,bebat,Recupel,Auvibel,PurchasePrice,Brand,CategoryId,VATPercId,Stock,EAN")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
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
