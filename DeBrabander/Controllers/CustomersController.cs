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
                Address Address = db.Addresses.Find(item.AddressId);
                PostalCode PostalCode = db.PostalCodes.Find(Address.PostalCodeId);

                civm.customer = item;
                civm.address = Address;
                civm.postalcode = PostalCode;

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
            
            Address add = db.Addresses.Find(cus.AddressId);
            PostalCode pos = db.PostalCodes.Find(add.PostalCodeId);

            CustomerDetailsViewModel cdvm = new CustomerDetailsViewModel();
            cdvm.customer = cus;
            cdvm.address = add;
            cdvm.postalcode = pos;

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
        public ActionResult Create([Bind(Include = "CustomerId,LastName,FirstName,CompanyName,Phone,CellPhone,Email,VATNumber,AccountNumber,Annotation,ContactName,ContactEmail,ContactCellPhone,CreationDate,Type,TAXLiability, StreetName, StreetNumber, Box, PostalCodeId, AddressId, PostalCodeNumber,Town")] CustomerCreateViewModel ccvm)
        {
            if (ModelState.IsValid)
            {
                Address add = new Address();
                Customer cus = new Customer();

                //tijdelijk tot postcodes erin zitten

                add.PostalCodeId = 1;
                add.StreetName = ccvm.address.StreetName;
                add.StreetNumber = ccvm.address.StreetNumber;
                add.Box = ccvm.address.Box;

                db.Addresses.Add(add);


                // is er een mogelijkheid om onderstaande code (custumer create view model -> customer) in een apparte BLL te doen ?
                //testcode met customer object
                //cus = ccvm.customer;
                //cus.FirstName = ccvm.customer.FirstName;


                cus.FirstName = ccvm.FirstName;
                cus.LastName = ccvm.LastName;
                cus.FirstName = ccvm.FirstName;
                cus.CompanyName = ccvm.CompanyName;
                cus.AddressId = ccvm.AddressId;
                cus.AccountNumber = ccvm.AccountNumber;
                cus.Annotation = ccvm.Annotation;
                cus.CellPhone = ccvm.CellPhone;
                cus.ContactCellPhone = ccvm.ContactCellPhone;
                cus.ContactEmail = ccvm.ContactEmail;
                cus.ContactName = ccvm.ContactName;
                cus.CreationDate = DateTime.Now;
                cus.Email = ccvm.Email;
                cus.Phone = ccvm.Phone;
                cus.TAXLiability = ccvm.TAXLiability;
                cus.Type = ccvm.Type;
                cus.VATNumber = ccvm.VATNumber;

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
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,LastName,FirstName,CompanyName,Phone,CellPhone,Email,VATNumber,AccountNumber,Annotation,ContactName,ContactEmail,ContactCellPhone,CreationDate,Type,TAXLiability")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
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
