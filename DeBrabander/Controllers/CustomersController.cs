﻿using System;
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
using PagedList;

namespace DeBrabander.Controllers
{
    public class CustomerBinderCreate : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            

            CustomerCreateViewModel obj = new CustomerCreateViewModel();
            obj.customer.Address = new Address();
            
            //obj.customer.CustomerDeliveryAddress = new List<CustomerDeliveryAddress>();
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
           
            //obj.customer.CustomerDeliveryAddress = new List<CustomerDeliveryAddress>();
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
            
            
            obj.customer.Type = objContext.Request.Form["customer.Type"];
            obj.customer.TAXLiability = objContext.Request.Form["customer.TAXLiability"];

            obj.customer.Address.AddressId = int.Parse(objContext.Request.Form["customer.Address.AddressId"]);
            obj.customer.Address.StreetName = objContext.Request.Form["customer.Address.StreetName"];
            obj.customer.Address.StreetNumber = int.Parse(objContext.Request.Form["customer.Address.StreetNumber"]);
            obj.customer.Address.Box = int.Parse(objContext.Request.Form["customer.Address.Box"]);

            //moet er nog uit
            
            obj.customer.Address.PostalCodeNumber = 2900;
            obj.customer.Address.Town = "Schoten";


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
                
                cus.Address = new Address();
                
                
                cus = customer.customer;
                
                UpdateModel(cus, "Customer");

                db.Entry(cus).State = EntityState.Modified;
                db.Entry(cus.Address).State = EntityState.Modified;
                
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

        // GET: Customers/AddDeliveryAddress/5
        public ActionResult AddDeliveryAddress(int? id, int? page, string sortOrder, string searchStringTown, string searchStringPostal)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //lijst vullen met adressen om weer te geven
            var addressList = from a in db.Addresses select a;

            // zoekveld toepassen op addresslist
            //if (!String.IsNullOrEmpty(searchString)) { addressList = addressList.Where(s => s.Town.ToUpper().Contains(searchString.ToUpper()) || 
            //    s.PostalCodeNumber==Convert.ToInt32(searchString) ||
            //    s.StreetName.ToUpper().Contains(searchString.ToUpper())); }
            if (!String.IsNullOrEmpty(searchStringTown))
            {
                addressList = addressList.Where(s => s.Town.ToUpper().Contains(searchStringTown.ToUpper()) ||
                s.StreetName.ToUpper().Contains(searchStringTown.ToUpper()));
            }



            //sorting parameters
            ViewBag.TownSortParm = String.IsNullOrEmpty(sortOrder) ? "town_desc" : "";
            ViewBag.PostalCodeSortParm = String.IsNullOrEmpty(sortOrder) ? "postal_desc" : "postal";


            switch (sortOrder)
            {
                case "town_desc":
                    addressList = addressList.OrderByDescending(s => s.Town);
                    break;
                case "postal":
                    addressList = addressList.OrderBy(s => s.PostalCodeNumber);
                    break;
                case "postal_desc":
                    addressList = addressList.OrderByDescending(s => s.PostalCodeNumber);
                    break;
                default:
                    addressList = addressList.OrderBy(s => s.Town);
                    break;
            }
       
            //gesorteerde info in viewbag + paged 
            //ViewBag.Addressess = addressList.ToPagedList(1, 2);
            ViewBag.Addressess = addressList.ToList();



            Customer customer = db.Customers.Find(id);
            CustomerAddDeliveryAddressViewModel cavm = new CustomerAddDeliveryAddressViewModel();
            cavm.customer = customer;
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View("AddDeliveryAddress",cavm);
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
