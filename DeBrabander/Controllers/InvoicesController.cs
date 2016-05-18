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
using DeBrabander.ViewModels.Invoices;
using PagedList;

namespace DeBrabander.Controllers
{
    public class InvoicesController : Controller
    {
        private Context db = new Context();

        // GET: Invoices
        public ActionResult Index(string searchInvoiceNumber, string currentFilterInvoiceNumber, string searchCustomer, string currentFilterCustomer, string searchDelivery, string currentFilterDelivery, int? page, string sortOrder)
        {
            InvoiceIndexViewModel iivm = new InvoiceIndexViewModel();
            var invoices = from i in db.Invoice select i;
            Invoice invoice = new Invoice();

            //ViewBag.CurrentSort = sortOrder;


            ViewBag.InvoiceSortParm = String.IsNullOrEmpty(sortOrder) ? "invoice_asc" : "";
            ViewBag.CustomerSortParm = sortOrder == "cust" ? "cust_desc" : "cust";


            if (searchCustomer != null || searchInvoiceNumber != null)
            {
                page = 1;
            }
            else
            {
                searchInvoiceNumber = currentFilterInvoiceNumber;
                searchCustomer = currentFilterCustomer;
                searchDelivery = currentFilterDelivery;
            }
            ViewBag.CurrentFilterInvoice = searchInvoiceNumber;
            ViewBag.CurrentFilterCustomer = searchCustomer;
            ViewBag.CurrentFilterDelivery = searchDelivery;


            if (!String.IsNullOrEmpty(searchInvoiceNumber))
            {
                invoices = invoices.Where(i => i.InvoiceNumber.ToString().Contains(searchInvoiceNumber));
            }
            if (!String.IsNullOrEmpty(searchCustomer))
            {
                invoices = invoices.Where(i => i.LastName.ToUpper().Contains(searchCustomer.ToUpper()) || i.FirstName.ToUpper().Contains(searchCustomer.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchDelivery))
            {
                invoices = invoices.Where(i => i.customerDeliveryAddress.DeliveryAddressInfo.ToUpper().Contains(searchDelivery.ToUpper()) || i.customerDeliveryAddress.StreetName.ToUpper().Contains(searchDelivery.ToUpper()) || i.customerDeliveryAddress.Town.ToUpper().Contains(searchDelivery.ToUpper()));
            }


            switch (sortOrder)
            {
                case "invoice_asc":
                    invoices = invoices.OrderBy(s => s.InvoiceNumber);
                    break;
                case "cust_desc":
                    invoices = invoices.OrderByDescending(s => s.LastName);
                    break;
                case "cust":
                    invoices = invoices.OrderBy(s => s.LastName);
                    break;
                default:
                    invoices = invoices.OrderByDescending(s => s.InvoiceNumber);
                    break;
            }

            var userDefinedInfo = db.UserDefinedSettings.Find(1);
            int pageSize = userDefinedInfo.IndexResultLength;
            int pageNumber = (page ?? 1);


            //ViewBag.Quotations = quotations.ToPagedList(pageNumber, pageSize);
            iivm.invoices = invoices.ToPagedList(pageNumber, pageSize);

            return View(iivm);
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoice.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceId,InvoiceNumber,TotalPrice,Date,Annotation,Active,CustomerId,LastName,FirstName,CellPhone,Email,StreetName,StreetNumber,Box,PostalCodeNumber,Town")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoice.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // opzoeken van de juiste quotation
            Invoice invoice = db.Invoice.Find(id);
            // alle klantgegevens ophalen van de offerte
            var customer = db.Customers.Find(invoice.CustomerId);
            // info opzoeken van het werfadres
            Address werfadres = new Address();
            werfadres.Box = invoice.customerDeliveryAddress.Box;
            werfadres.PostalCodeNumber = invoice.customerDeliveryAddress.PostalCodeNumber;
            werfadres.StreetName = invoice.customerDeliveryAddress.StreetName;
            werfadres.StreetNumber = invoice.customerDeliveryAddress.StreetNumber;
            werfadres.Town = invoice.customerDeliveryAddress.Town;

            // vullen van viewmodel
            InvoiceEditViewModel ievm = new InvoiceEditViewModel();
            ievm.invoice = invoice;
            ievm.customer = customer;
            ievm.address = werfadres;



            if (invoice == null)
            {
                return HttpNotFound();
            }
            // omgezet naar model
            //ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName");
            //var quotDetList = from q in db.QuotationDetails select q;
            //quotDetList = quotDetList.Where(q => q.QuotationId == id);
            //ViewBag.QuotationDetail = quotDetList.ToList();
            return View(ievm);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceId,InvoiceNumber,TotalPrice,Date,Annotation,Active,CustomerId,LastName,FirstName,CellPhone,Email,StreetName,StreetNumber,Box,PostalCodeNumber,Town")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoice.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoice.Find(id);
            db.Invoice.Remove(invoice);
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
