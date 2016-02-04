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
using DeBrabander.ViewModels.VATs;

namespace DeBrabander.Controllers
{
    public class VATsController : Controller
    {
        private Context db = new Context();

        // GET: VATs
        public ActionResult Index()
        {
            List<VatIndexViewModel> VATVMList = new List<VatIndexViewModel>();
            List<VAT> vats = new List<VAT>(db.VATs.ToList());

            foreach (var vat in vats)
            {
                VatIndexViewModel vivm = new VatIndexViewModel();
                vivm.VATPercId = vat.VATPercId;
                vivm.VATValue = vat.VATValue;
                VATVMList.Add(vivm);
            }
            return View(VATVMList);
        }

        // GET: VATs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VAT vAT = db.VATs.Find(id);
            if (vAT == null)
            {
                return HttpNotFound();
            }
            VatDetailsViewModel vdvm = new VatDetailsViewModel();
            vdvm.VATPercId = vAT.VATPercId;
            vdvm.VATValue = vAT.VATValue;

            return View(vdvm);
        }

        // GET: VATs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VATs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VATPercId,VATValue")] VatCreateViewModel vatcvm)
        {
            if (ModelState.IsValid)
            {
                VAT vat = new VAT();
                vat.VATValue = vatcvm.VATValue;

                db.VATs.Add(vat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vatcvm);
        }

        // GET: VATs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VAT vAT = db.VATs.Find(id);
            VatEditViewModel vevm = new VatEditViewModel();
            vevm.VATPercId = vAT.VATPercId;
            vevm.VATValue = vAT.VATValue;

            if (vAT == null)
            {
                return HttpNotFound();
            }
            return View(vevm);
        }

        // POST: VATs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VATPercId,VATValue")] VatCreateViewModel vatcvm)
        {
            if (ModelState.IsValid)
            {
                VAT vat = new VAT();
                vat.VATPercId = vatcvm.VATPercId;
                vat.VATValue = vatcvm.VATValue;


                db.Entry(vat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vatcvm);
        }

        // GET: VATs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VAT vAT = db.VATs.Find(id);
            if (vAT == null)
            {
                return HttpNotFound();
            }
            return View(vAT);
        }

        // POST: VATs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VAT vAT = db.VATs.Find(id);
            db.VATs.Remove(vAT);
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
