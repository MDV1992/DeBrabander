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
    public class InvoiceDetailsController : Controller
    {
        private Context db = new Context();

        // GET: InvoiceDetails
        public ActionResult Index()
        {
            var invoiceDetails = db.InvoiceDetails.Include(i => i.Invoice).Include(i => i.VAT);
            return View(invoiceDetails.ToList());
        }

        // GET: InvoiceDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceDetail invoiceDetail = db.InvoiceDetails.Find(id);
            if (invoiceDetail == null)
            {
                return HttpNotFound();
            }
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Create
        public ActionResult Create()
        {
            ViewBag.InvoiceId = new SelectList(db.Invoice, "InvoiceId", "Annotation");
            ViewBag.VATPercId = new SelectList(db.VATs, "VATPercId", "VATPercId");
            return View();
        }

        // POST: InvoiceDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceDetailId,Quantity,InvoiceId,ProductId,ProductName,ProductCode,Description,PriceExVAT,Reprobel,Bebat,Recupel,Auvibel,Brand,CategoryId,VATPercId,TotalExVat,TotalIncVat")] InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceDetails.Add(invoiceDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InvoiceId = new SelectList(db.Invoice, "InvoiceId", "Annotation", invoiceDetail.InvoiceId);
            ViewBag.VATPercId = new SelectList(db.VATs, "VATPercId", "VATPercId", invoiceDetail.VATPercId);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceDetail invoiceDetail = db.InvoiceDetails.Find(id);
            if (invoiceDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvoiceId = new SelectList(db.Invoice, "InvoiceId", "Annotation", invoiceDetail.InvoiceId);
            ViewBag.VATPercId = new SelectList(db.VATs, "VATPercId", "VATPercId", invoiceDetail.VATPercId);
            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceDetailId,Quantity,InvoiceId,ProductId,ProductName,ProductCode,Description,PriceExVAT,Reprobel,Bebat,Recupel,Auvibel,Brand,CategoryId,VATPercId,TotalExVat,TotalIncVat")] InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoiceDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvoiceId = new SelectList(db.Invoice, "InvoiceId", "Annotation", invoiceDetail.InvoiceId);
            ViewBag.VATPercId = new SelectList(db.VATs, "VATPercId", "VATPercId", invoiceDetail.VATPercId);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceDetail invoiceDetail = db.InvoiceDetails.Find(id);
            if (invoiceDetail == null)
            {
                return HttpNotFound();
            }
            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvoiceDetail invoiceDetail = db.InvoiceDetails.Find(id);
            db.InvoiceDetails.Remove(invoiceDetail);
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
