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

namespace DeBrabander.Controllers
{
    public class QuotationDetailsController : Controller
    {
        private Context db = new Context();

        // GET: QuotationDetails
        public ActionResult Index()
        {
            return View(db.QuotationDetails.ToList());
        }

        // GET: QuotationDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuotationDetail quotationDetail = db.QuotationDetails.Find(id);
            if (quotationDetail == null)
            {
                return HttpNotFound();
            }
            return View(quotationDetail);
        }

        // GET: QuotationDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuotationDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuotationDetailId,Quantity,QuotationId,ProductId,ProductName,ProductCode,Description,PriceExVAT,Reprobel,Bebat,Recupel,Auvibel,Brand,CategoryId,VATPercId")] QuotationDetail quotationDetail)
        {
            if (ModelState.IsValid)
            {
                db.QuotationDetails.Add(quotationDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quotationDetail);
        }

        // GET: QuotationDetails/Edit/5
        public ActionResult Edit(int? id, int? edit)
        {
            ViewBag.VAT = new SelectList(db.QuotationDetails, "VATPercId", "VATValue");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuotationDetail quotationDetail = db.QuotationDetails.Find(id);
            if (quotationDetail == null)
            {
                return HttpNotFound();
            }
            // zet de edit waarde op 0 of 1 (zodat we weten dat hij van Edit of vanuit addproducts komt)
            ViewBag.edit = 0;
            if (edit != null)
            {
                ViewBag.edit = edit;
            }
            
            return View(quotationDetail);
        }

        // POST: QuotationDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuotationDetailId,Quantity,QuotationId,ProductId,ProductName,ProductCode,Description,PriceExVAT,Reprobel,Bebat,Recupel,Auvibel,Brand,CategoryId,VATPercId")] QuotationDetail quotationDetail, int editValue)
        {
            Product prod = new Product();
            prod = db.Products.Find(quotationDetail.ProductId);
            quotationDetail.ProductName = prod.ProductName;
            quotationDetail.TotalExVat = quotationDetail.PriceExVAT * quotationDetail.Quantity;
            //quotationDetail.TotalIncVat = quotationDetail.TotalExVat * (1 + (quotationDetail.VAT.VATValue / 100));
            if (ModelState.IsValid)
            {
                db.Entry(quotationDetail).State = EntityState.Modified;
                db.SaveChanges();
                if (editValue == 1)
                {
                    return RedirectToAction("Edit", "Quotations", new { id = quotationDetail.QuotationId });
                }
                return RedirectToAction("AddProducts", "Quotations", new { id = quotationDetail.QuotationId });
            }
            return View(quotationDetail);
        }

        // GET: QuotationDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuotationDetail quotationDetail = db.QuotationDetails.Find(id);
            if (quotationDetail == null)
            {
                return HttpNotFound();
            }
            // uitleg returnUrl http://stackoverflow.com/questions/9772947/c-sharp-asp-net-mvc-return-to-previous-page
            ViewBag.returnUrl = Request.UrlReferrer;
            return View(quotationDetail);
        }

        // POST: QuotationDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            QuotationDetail quotationDetail = db.QuotationDetails.Find(id);
            db.QuotationDetails.Remove(quotationDetail);

            db.SaveChanges();
            return Redirect(returnUrl);
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
