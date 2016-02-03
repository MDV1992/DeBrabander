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
using DeBrabander.ViewModels.Customers;

namespace DeBrabander.Controllers
{
    public class CustomerBinderCreate : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            

            CustomerCreateViewModel obj = new CustomerCreateViewModel();
            obj.customer.Address = new Address();
            obj.customer.Address.PostalCode = new PostalCode();
            obj.customer.CustomerDeliveryAddress = new List<CustomerDeliveryAddress>();
            obj.customer.LastName = objContext.Request.Form["customer.LastName"];
            obj.customer.FirstName = objContext.Request.Form["customer.FirstName"];
            obj.customer.CompanyName = objContext.Request.Form["customer.CompanyName"];
            obj.customer.Phone = objContext.Request.Form["customer.Phone"];
            obj.customer.CellPhone = objContext.Request.Form["customer.CellPhone"];
            obj.customer.Email = objContext.Request.Form["customer.Email"];
            obj.customer.VATNumber = objContext.Request.Form["customer.VATNumber"];
            obj.customer.AccountNumber = objContext.Request.Form["customer.AccountNumber"];
            obj.customer.Annotation = objContext.Request.Form["customer.Annotation"];
            obj.customer.ContactName = objContext.Request.Form["customer.ContactName"];
            obj.customer.ContactEmail = objContext.Request.Form["customer.ContactEmail"];
            obj.customer.ContactCellPhone = objContext.Request.Form["customer.ContactCellPhone"];
            obj.customer.CreationDate = DateTime.Now;
            obj.customer.Type = objContext.Request.Form["customer.Type"];
            obj.customer.TAXLiability = objContext.Request.Form["customer.TAXLiability"];
            
            obj.customer.Address.StreetName = objContext.Request.Form["customer.Address.StreetName"];
            obj.customer.Address.StreetNumber = int.Parse(objContext.Request.Form["customer.Address.StreetNumber"]);
            obj.customer.Address.Box = int.Parse(objContext.Request.Form["customer.Address.Box"]);

            //moet er nog uit
            obj.customer.Address.PostalCode.PostalCodeId = 1;
            

            return obj;
            
        }
    }

    public class CustomerBinderEdit : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;


            CustomerEditViewModel obj = new CustomerEditViewModel();
            obj.customer.Address = new Address();
            obj.customer.Address.PostalCode = new PostalCode();
            obj.customer.CustomerDeliveryAddress = new List<CustomerDeliveryAddress>();
            obj.customer.LastName = objContext.Request.Form["customer.LastName"];
            obj.customer.FirstName = objContext.Request.Form["customer.FirstName"];
            obj.customer.CompanyName = objContext.Request.Form["customer.CompanyName"];
            obj.customer.Phone = objContext.Request.Form["customer.Phone"];
            obj.customer.CellPhone = objContext.Request.Form["customer.CellPhone"];
            obj.customer.Email = objContext.Request.Form["customer.Email"];
            obj.customer.VATNumber = objContext.Request.Form["customer.VATNumber"];
            obj.customer.AccountNumber = objContext.Request.Form["customer.AccountNumber"];
            obj.customer.Annotation = objContext.Request.Form["customer.Annotation"];
            obj.customer.ContactName = objContext.Request.Form["customer.ContactName"];
            obj.customer.ContactEmail = objContext.Request.Form["customer.ContactEmail"];
            obj.customer.ContactCellPhone = objContext.Request.Form["customer.ContactCellPhone"];
            obj.customer.CreationDate = DateTime.Now;
            obj.customer.Type = objContext.Request.Form["customer.Type"];
            obj.customer.TAXLiability = objContext.Request.Form["customer.TAXLiability"];

            obj.customer.Address.StreetName = objContext.Request.Form["customer.Address.StreetName"];
            obj.customer.Address.StreetNumber = int.Parse(objContext.Request.Form["customer.Address.StreetNumber"]);
            obj.customer.Address.Box = int.Parse(objContext.Request.Form["customer.Address.Box"]);

            //moet er nog uit
            obj.customer.Address.PostalCode.PostalCodeId = 1;
            obj.customer.Address.PostalCode.PostalCodeNumber = 2900;
            obj.customer.Address.PostalCode.Town = "Schoten";


            return obj;

        }
    }



    public class CustomersController : Controller
    {
        private Context db = new Context();

        // GET: Customers
        public ActionResult Index()
        {
            List<CustomerIndexViewModel> custumorVMList = new List<CustomerIndexViewModel>();
            List<Customer> customers = new List<Customer>(db.Customers.ToList());
            
            foreach (var item in customers)
            {
                CustomerIndexViewModel civm = new CustomerIndexViewModel();                
                civm.customer = item;               
                custumorVMList.Add(civm);
            }

            return View(custumorVMList);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer cus = db.Customers.Find(id);
            if (cus == null)
            {
                return HttpNotFound();
            }
            
            CustomerDetailsViewModel cdvm = new CustomerDetailsViewModel();
            cdvm.customer = cus;
            
            return View(cdvm);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            
            ViewBag.PostalCodesId = new SelectList(db.PostalCodes, "PostalCodeId", "Town");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "CustomerId,LastName,FirstName,CompanyName,Phone,CellPhone,Email,VATNumber,AccountNumber,Annotation,ContactName,ContactEmail,ContactCellPhone,CreationDate,Type,TAXLiability, StreetName, StreetNumber, Box, PostalCodeId, AddressId, PostalCodeNumber,Town")] CustomerCreateViewModel ccvm)
        public ActionResult Create([ModelBinder(typeof(CustomerBinderCreate))] CustomerCreateViewModel ccvm)
        {
            if (ModelState.IsValid)
            {
                Customer cus = new Customer();
                cus = ccvm.customer;
                db.Customers.Add(cus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ccvm);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            CustomerEditViewModel cevm = new CustomerEditViewModel();
            cevm.customer = customer;
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(cevm);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([ModelBinder(typeof(CustomerBinderEdit))] CustomerEditViewModel customer)
        {
            
            if (ModelState.IsValid)
            {
                Customer cus = new Customer();                
                //cus.Address = new Address();
                //cus.Address.PostalCode = new PostalCode();
                //cus.CustomerDeliveryAddress = new List<CustomerDeliveryAddress>();
                cus = customer.customer;
                UpdateModel(cus, "Customer");
                //UpdateModel(cus.Address, "Address");

                db.Entry(cus).State = EntityState.Modified;
                //db.Entry(cus.Address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
