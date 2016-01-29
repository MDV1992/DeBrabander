using System;
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
            List<ProductIndexViewModel> productVMList = new List<ProductIndexViewModel>();
            List<Product> products = new List<Product>(db.Products.ToList());
            
            foreach (var prod in products)
            {
                ProductIndexViewModel pivm = new ProductIndexViewModel();
                pivm.ProductId = prod.ProductId;
                pivm.ProductName = prod.ProductName;
                pivm.ProductCode = prod.ProductCode;
                pivm.Remark = prod.Remark;
                pivm.Description = prod.Description;
                pivm.PriceExVAT = prod.PriceExVAT;
                pivm.Reprobel = prod.Reprobel;
                pivm.Bebat = prod.Bebat;
                pivm.Recupel = prod.Recupel;
                pivm.Auvibel = prod.Auvibel;
                pivm.PurchasePrice = prod.PurchasePrice;
                pivm.Brand = prod.Brand;
                Category cat = db.Categories.Find(prod.CategoryId);
                VAT vat = db.VATs.Find(prod.VATPercId);
                pivm.CategoryName = cat.CategoryName;
                pivm.VATValue = vat.VATValue;
                pivm.Stock = prod.Stock;
                pivm.EAN = prod.EAN;
                pivm.StockControl = prod.StockControl;
                productVMList.Add(pivm);
                
            }

           
            return View(productVMList);

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
           
            ViewBag.VATPercList= new SelectList(db.VATs, "VATPercId", "VATValue"); 
            ViewBag.CategorySelect = new SelectList(db.Categories, "CategoryId", "CategoryName");
          
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductCode,Remark,Description,PriceExVAT,Reprobel,Bebat,Recupel,Auvibel,PurchasePrice,Brand,CategoryId,VATPercId,Stock,EAN")] ProductCreateViewModel product)
        {

            if (ModelState.IsValid)
            {
                Product prod = new Product();
                prod.ProductName = product.ProductName;
                prod.ProductCode = product.ProductCode;
                prod.Remark = product.Remark;
                prod.Description = product.Description;
                prod.PriceExVAT = product.PriceExVAT;
                prod.Reprobel = product.Reprobel;
                prod.Bebat = product.Bebat;
                prod.Recupel = product.Recupel;
                prod.Auvibel = product.Auvibel;
                prod.PurchasePrice = product.PurchasePrice;
                prod.Brand = product.Brand;
                prod.CategoryId = product.CategoryId;
                prod.VATPercId = product.VATPercId;
                prod.Stock = product.Stock;
                prod.EAN = product.EAN;

                db.Products.Add(prod);
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

            ViewBag.VATPercList = new SelectList(db.VATs, "VATPercId", "VATValue");
            ViewBag.CategorySelect = new SelectList(db.Categories, "CategoryId", "CategoryName");
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
