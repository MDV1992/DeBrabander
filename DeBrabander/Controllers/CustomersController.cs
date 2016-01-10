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
                civm.FirstName = item.FirstName;
                civm.LastName = item.LastName;
                civm.CustomerId = item.CustomerId;
                civm.ContactCellPhone = item.CellPhone;
                civm.Email = item.Email;
                civm.CompanyName = item.CompanyName;
                civm.VATNumber = item.VATNumber;
                Address Address = db.Addresses.Find(item.AddressId);
                PostalCode PostalCode = db.PostalCodes.Find(Address.PostalCodeId);
                civm.StreetName = Address.StreetName;
                civm.StreetNumber = Address.StreetNumber;
                civm.Town = PostalCode.Town;
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
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            
            Address address = db.Addresses.Find(customer.AddressId);
            PostalCode postalCode = db.PostalCodes.Find(address.PostalCodeId);

            CustomerDetailsViewModel cdvm = new CustomerDetailsViewModel();

            cdvm.FirstName = customer.FirstName;
            cdvm.LastName = customer.LastName;
            cdvm.StreetName = address.StreetName;
            cdvm.CompanyName = customer.CompanyName;
            cdvm.AccountNumber = customer.AccountNumber;
            cdvm.Annotation = customer.Annotation;
            cdvm.CellPhone = customer.CellPhone;
            cdvm.ContactCellPhone = customer.ContactCellPhone;
            cdvm.ContactEmail = customer.ContactEmail;
            cdvm.ContactName = customer.ContactName;
            cdvm.CreationDate = customer.CreationDate;
            cdvm.Email = customer.Email;
            cdvm.Phone = customer.Phone;
            cdvm.TAXLiability = customer.TAXLiability;
            cdvm.Type = customer.Type;
            cdvm.VATNumber = customer.VATNumber;
            cdvm.CustomerId = customer.CustomerId;

            cdvm.AddressId = customer.AddressId;
            cdvm.StreetNumber = address.StreetNumber;
            cdvm.StreetNumber = address.StreetNumber;
            cdvm.Box = address.Box;
            cdvm.PostalCodeId = address.PostalCodeId;

            cdvm.PostalCodeNumber = postalCode.PostalCodeNumber;
            cdvm.Town = postalCode.Town;
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
