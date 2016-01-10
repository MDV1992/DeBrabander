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
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
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
            int AId = customer.AddressId;
            Address Address = db.Addresses.Find(AId);
            int? PId = Address.PostalCodeId;
            PostalCode PostalCode = db.PostalCodes.Find(PId);

            CustomerDetailsViewModel cdvm = new CustomerDetailsViewModel();

            cdvm.FirstName = customer.FirstName;
            cdvm.LastName = customer.LastName;
            cdvm.StreetName = Address.StreetName;
            cdvm.StreetNumber = Address.StreetNumber;
            cdvm.PostalCodeNumber = PostalCode.PostalCodeNumber;
            cdvm.Town = PostalCode.Town;
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
        public ActionResult Create([Bind(Include = "CustomerId,LastName,FirstName,CompanyName,Phone,CellPhone,Email,VATNumber,AccountNumber,Annotation,ContactName,ContactEmail,ContactCellPhone,CreationDate,Type,TAXLiability, StreetName, StreetNumber, Box, PostalCodeId, AddressId, PostalCodeNumber,Town")] CustomerCreateViewModel customer)
        {
            if (ModelState.IsValid)
            {
                Address address = new Address();
                Customer cus = new Customer();

                //tijdelijk tot postcodes erin zitten

                address.PostalCodeId = 1;
                address.StreetName = customer.StreetName;
                address.StreetNumber = customer.StreetNumber;
                address.Box = customer.Box;

                db.Addresses.Add(address);


                // is er een mogelijkheid om onderstaande code (custumer create view model -> customer) in een apparte BLL te doen ?
                cus.LastName = customer.LastName;
                cus.FirstName = customer.FirstName;
                cus.CompanyName = customer.CompanyName;
                cus.AddressId = address.AddressId;
                cus.AccountNumber = customer.AccountNumber;
                cus.Annotation = customer.Annotation;
                cus.CellPhone = customer.CellPhone;
                cus.ContactCellPhone = customer.ContactCellPhone;
                cus.ContactEmail = customer.ContactEmail;
                cus.ContactName = customer.ContactName;
                cus.CreationDate = DateTime.Now;
                cus.Email = customer.Email;
                cus.Phone = customer.Phone;
                cus.TAXLiability = customer.TAXLiability;
                cus.Type = customer.Type;
                cus.VATNumber = customer.VATNumber;   

                db.Customers.Add(cus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
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
