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
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace DeBrabander.Controllers
{
    public class InvoicesController : Controller
    {
        private Context db = new Context();

        // GET: Invoices
        public ActionResult Index(string searchInvoiceNumber, string currentFilterInvoiceNumber, string searchCustomer, string currentFilterCustomer, string searchDelivery, string currentFilterDelivery, int? page, string sortOrder)
        {
            InvoiceIndexViewModel iivm = new InvoiceIndexViewModel();
            var invoices = from i in db.Invoices select i;
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
            Invoice invoice = db.Invoices.Find(id);
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
                db.Invoices.Add(invoice);
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
            Invoice invoice = db.Invoices.Find(id);
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
            Invoice invoice = db.Invoices.Find(id);
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
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CRInvoice(int id)
        {

            //Lijsten maken en vullen
            List<SingleInvoiceCRViewModel> CRSingleInvoiceListVM = new List<SingleInvoiceCRViewModel>();
            SingleInvoiceCRViewModel CRSO = new SingleInvoiceCRViewModel();

            Invoice invoice = db.Invoices.Find(id);
            Company company = db.Companies.Find(1);
            Customer cus = db.Customers.Find(invoice.CustomerId);


            //vulling Header info
            //company
            CRSO.BIC = company.BIC;
            CRSO.CompanyId = company.CompanyId;
            CRSO.CompanyName = company.CompanyName;
            CRSO.Country = company.Country;
            CRSO.District = company.District;
            CRSO.EmailCompany = company.Email;
            CRSO.Iban = company.Iban;
            CRSO.Mobile = company.Mobile;
            CRSO.Phone = company.Phone;
            CRSO.Postalcode = company.Postalcode;
            CRSO.Street = company.Street;
            CRSO.VatNumber = company.VatNumber;
            CRSO.Website = company.Website;

            //quotation
            CRSO.Annotation = invoice.Annotation;
            CRSO.Box = invoice.Box;
            CRSO.CellPhone = invoice.CellPhone;
            CRSO.CustomerId = invoice.CustomerId;
            CRSO.Date = invoice.Date;
            CRSO.EmailCustomer = invoice.Email;
            CRSO.FirstName = invoice.FirstName;
            CRSO.LastName = invoice.LastName;
            CRSO.PostalCodeNumber = invoice.PostalCodeNumber;
            CRSO.InvoiceID = invoice.InvoiceId;
            CRSO.InvoiceNumber = invoice.InvoiceNumber;
            CRSO.StreetName = invoice.StreetName;
            CRSO.StreetNumber = invoice.StreetNumber;
            CRSO.TotalPrice = invoice.TotalPrice;
            CRSO.Town = invoice.Town;
            CRSO.VATnumberCustomer = cus.VATNumber;

            //customer contact info
            CRSO.ContactCellPhone = cus.ContactCellPhone;
            CRSO.ContactEmail = cus.ContactEmail;
            CRSO.ContactName = cus.ContactName;

            //delivery info
            CRSO.DeliveryAddressInfo = invoice.customerDeliveryAddress.DeliveryAddressInfo;
            CRSO.PostalCodeNumberTown = invoice.customerDeliveryAddress.PostalCodeNumber + " " + invoice.customerDeliveryAddress.Town;
            CRSO.StreetNameNumberBox = invoice.customerDeliveryAddress.StreetName + " " + invoice.customerDeliveryAddress.StreetNumber + " " + invoice.customerDeliveryAddress.Box;

            //vulling details info
            foreach (var item in invoice.InvoiceDetail)
            {
                CRSO.ProductCode = item.ProductCode;
                CRSO.PriceExVAT = item.PriceExVAT;
                CRSO.Quantity = item.Quantity;
                CRSO.Description = item.Description;
                CRSO.VATValue = Convert.ToInt16(item.VAT.VATValue);
                CRSO.Auvibel = item.Auvibel;
                CRSO.Recupel = item.Recupel;
                CRSO.Reprobel = item.Reprobel;
                CRSO.Bebat = item.Bebat;
                CRSO.ProductCode = item.ProductCode;
                CRSO.ProductName = item.ProductName;
                CRSingleInvoiceListVM.Add(CRSO);
                SingleInvoiceCRViewModel empty = new SingleInvoiceCRViewModel();
                CRSO = empty;
            }



            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/Invoice"), "SingleInvoice.rpt"));
            rd.SetDataSource(CRSingleInvoiceListVM);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                string pdfName = "Order - " + invoice.InvoiceNumber + " - " + invoice.FullName.ToString() + ".pdf";
                return File(stream, "application/pdf", pdfName);
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
