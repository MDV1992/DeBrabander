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
using DeBrabander.ViewModels;
using PagedList;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

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
                pivm.Active = prod.Active;
                if (pivm.Active == true)
                {
                    productVMList.Add(pivm);
                }
                
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
            pdvm.Active = product.Active;

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
                prod.Active = product.Active;

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
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductCode,Remark,Description,PriceExVAT,Reprobel,Bebat,Recupel,Auvibel,PurchasePrice,Brand,CategoryId,VATPercId,Stock,EAN")] Product product)
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

        public ActionResult CRAllProducts(int? id)
        {
            List<Product> allProducts = new List<Product>();
            allProducts = db.Products.ToList();

            List<Company> company = new List<Company>();
            company = db.Companies.ToList();

            List<AllProductsCRViewModel> allProductsCR = new List<AllProductsCRViewModel>();
            foreach (var item in allProducts)
            {
                var allProductCR = new AllProductsCRViewModel();                
                allProductCR.ProductId = item.ProductId;
                allProductCR.ProductName = item.ProductName;
                allProductCR.ProductCode = item.ProductCode;
                allProductCR.Brand = item.Brand;
                allProductCR.Auvibel = item.Auvibel;
                allProductCR.Bebat = item.Bebat;
                allProductCR.Recupel = item.Recupel;
                allProductCR.Reprobel = item.Reprobel;
                allProductCR.PurchasePrice = item.PurchasePrice;
                allProductCR.PriceExVAT = item.PriceExVAT;
                allProductCR.VATPercId = item.VATPercId;
                allProductCR.Remark = item.Remark;
                allProductCR.Stock = item.Stock;
                allProductCR.Description = item.Description;
                allProductCR.EAN = item.EAN;
                allProductCR.CategoryId = item.CategoryId;
                allProductCR.CategoryName = item.Category.CategoryName;
                allProductCR.VATValue = item.VAT.VATValue;
                allProductCR.Active = item.Active;
           
                

                allProductsCR.Add(allProductCR);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "AllProductsMain.rpt"));
            rd.OpenSubreport("Header.rpt").SetDataSource(company);
            rd.OpenSubreport("allProductsSub.rpt").SetDataSource(allProductsCR);
            //rd.SetDataSource(company);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "All_Products.pdf");
            }
            catch (Exception ex)
            {
                if (ex.Data == null)
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }
        }

        public ActionResult CRProduct(int id)
        {
            List<Product> product = new List<Product>();
            product = db.Products.ToList();

            List<Company> company = new List<Company>();
            company = db.Companies.ToList();

            List<ProductCRViewModel> ProductCR = new List<ProductCRViewModel>();
            foreach (var item in product)
            {
                var productCR = new ProductCRViewModel();
                productCR.ProductId = item.ProductId;
                productCR.ProductName = item.ProductName;
                productCR.ProductCode = item.ProductCode;
                productCR.Brand = item.Brand;
                productCR.Auvibel = item.Auvibel;
                productCR.Bebat = item.Bebat;
                productCR.Recupel = item.Recupel;
                productCR.Reprobel = item.Reprobel;
                productCR.PurchasePrice = item.PurchasePrice;
                productCR.PriceExVAT = item.PriceExVAT;
                productCR.VATPercId = item.VATPercId;
                productCR.Remark = item.Remark;
                productCR.Stock = item.Stock;
                productCR.Description = item.Description;
                productCR.EAN = item.EAN;
                productCR.CategoryId = item.CategoryId;
                productCR.CategoryName = item.Category.CategoryName;
                productCR.VATValue = item.VAT.VATValue;
                productCR.Active = item.Active;




                ProductCR.Add(productCR);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "AllProductsMain.rpt"));
            rd.OpenSubreport("Header.rpt").SetDataSource(company);
            rd.OpenSubreport("allProductsSub.rpt").SetDataSource(ProductCR);
            //rd.SetDataSource(company);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Product_spec.pdf");
            }
            catch (Exception ex)
            {
                if (ex.Data == null)
                {
                    throw;
                }
                else
                {
                    throw;
                }
            }
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
