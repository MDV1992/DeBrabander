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
using DeBrabander.ViewModels.Quotations;

namespace DeBrabander.Controllers
{
    public class QuotationBinderCreate : IModelBinder
    {
        private Context db2 = new Context();
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;



            
            QuotationCreateViewModel obj = new QuotationCreateViewModel();
            obj.quotation.Customer = new Customer();
            int tempid = int.Parse(objContext.Request.Form["quotation.Customer.CustomerId"]);
            obj.quotation.Customer.CustomerId = tempid;
            obj.quotation.Active = bool.Parse(objContext.Request.Form.GetValues("quotation.Active")[0]);
            obj.quotation.Annotation = objContext.Request.Form["quotation.Annotation"];
            obj.quotation.Date = DateTime.Parse(objContext.Request.Form["quotation.Date"]);
            obj.quotation.ExpirationDate = DateTime.Parse(objContext.Request.Form["quotation.ExpirationDate"]);
            obj.quotation.QuotationNumber = int.Parse(objContext.Request.Form["quotation.QuotationNumber"]);
            



            //obj.customer.LastName = objContext.Request.Form["customer.LastName"];
            //obj.customer.FirstName = objContext.Request.Form["customer.FirstName"];
            //obj.customer.CompanyName = objContext.Request.Form["customer.CompanyName"];
            //obj.customer.Phone = objContext.Request.Form["customer.Phone"];
            //obj.customer.CellPhone = objContext.Request.Form["customer.CellPhone"];
            //obj.customer.Email = objContext.Request.Form["customer.Email"];
            //obj.customer.VATNumber = objContext.Request.Form["customer.VATNumber"];
            //obj.customer.AccountNumber = objContext.Request.Form["customer.AccountNumber"];
            //obj.customer.Annotation = objContext.Request.Form["customer.Annotation"];
            //obj.customer.ContactName = objContext.Request.Form["customer.ContactName"];
            //obj.customer.ContactEmail = objContext.Request.Form["customer.ContactEmail"];
            //obj.customer.ContactCellPhone = objContext.Request.Form["customer.ContactCellPhone"];
            //obj.customer.CreationDate = DateTime.Now;
            //obj.customer.Type = objContext.Request.Form["customer.Type"];
            //obj.customer.TAXLiability = objContext.Request.Form["customer.TAXLiability"];

            //obj.customer.Address.StreetName = objContext.Request.Form["customer.Address.StreetName"];
            //obj.customer.Address.StreetNumber = int.Parse(objContext.Request.Form["customer.Address.StreetNumber"]);
            //obj.customer.Address.Box = int.Parse(objContext.Request.Form["customer.Address.Box"]);

            //moet er nog uit
            //obj.customer.Address.PostalCode.PostalCodeId = 1;


            return obj;

        }
    }

    public class QuotationsController : Controller
    {
        private Context db = new Context();

        // GET: Quotations
        public ActionResult Index(string searchString)
        {
            var quotations = from q in db.Quotations select q;
            if (!String.IsNullOrEmpty(searchString))
            {
                quotations = quotations.Where(q => q.QuotationNumber.ToString().Contains(searchString));
            }
            return View(db.Quotations.ToList());
        }

        // GET: Quotations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            return View(quotation);
        }

        // GET: Quotations/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerId", "LastName");
            Quotation quotation = new Quotation();
            //db.Quotations.Add(quotation);
            //db.SaveChanges();
            //quotation.QuotationNumber = quotation.QuotationId;
            

            quotation.QuotationNumber = 1;
            quotation.Active = true;
            quotation.Date = DateTime.Now;
            quotation.ExpirationDate = quotation.Date.AddMonths(1);
            TempData["testing"] = 25;

            //db.SaveChanges();
            
            QuotationCreateViewModel qcvm = new QuotationCreateViewModel();
            qcvm.quotation = quotation;
            
            
            
            
            return View(qcvm);
        }

        // POST: Quotations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([ModelBinder(typeof(QuotationBinderCreate))]  QuotationCreateViewModel qcvm)
        {
            if (ModelState.IsValid)
            {
                Quotation quotation = new Quotation();
                Customer cus = new Customer();
                
                cus = db.Customers.Find(qcvm.quotation.Customer.CustomerId);

                quotation.Customer = new Customer();
                quotation.Customer = cus;

                quotation.customerDeliveryAddress = new CustomerDeliveryAddress();
                
                quotation = qcvm.quotation;

                db.Quotations.Add(quotation);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(qcvm);
        }

        // GET: Quotations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName");
            return View(quotation);
        }

        // POST: Quotations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuotationID,QuotationNumber,TotalPrice,Date,ExpirationDate,Annotation,Active, CustomerId")] Quotation quotation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quotation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quotation);
        }

        // GET: Quotations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            return View(quotation);
        }

        // POST: Quotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quotation quotation = db.Quotations.Find(id);
            db.Quotations.Remove(quotation);
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
