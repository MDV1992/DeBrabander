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
        public ActionResult Create([Bind(Include = "QuotationDetailId,ProductCode,ProductName,UnitPrice,Quantity")] QuotationDetail quotationDetail)
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
        public ActionResult Edit(int? id)
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

        // POST: QuotationDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuotationDetailId,ProductCode,ProductName,UnitPrice,Quantity")] QuotationDetail quotationDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quotationDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return View(quotationDetail);
        }

        // POST: QuotationDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuotationDetail quotationDetail = db.QuotationDetails.Find(id);
            db.QuotationDetails.Remove(quotationDetail);
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
