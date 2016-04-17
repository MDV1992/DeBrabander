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
using DeBrabander.ViewModels.Orders;
using PagedList;

namespace DeBrabander.Controllers
{
    public class OrdersController : Controller
    {
        private Context db = new Context();

        // GET: Orders
        public ActionResult Index(string searchOrderNumber, string currentFilterOrderNumber, string searchCustomer, string currentFilterCustomer, string searchDelivery, string currentFilterDelivery, bool? searchNonActive, bool? currentFilterNonActive, int? page, string sortOrder)
        {
            // viewmodel aanmaken + vullen tijdelijke lijst
            if (searchNonActive == null) { searchNonActive = false; }
            if (currentFilterNonActive == null) { currentFilterNonActive = false; }
            OrderIndexViewModel oivm = new OrderIndexViewModel();
            var orders = from o in db.Orders select o;
            Order order = new Order();

            
            ViewBag.OrderSortParm = String.IsNullOrEmpty(sortOrder) ? "order_desc" : "";
            ViewBag.CustomerSortParm = sortOrder == "cust" ? "cust_desc" : "cust";


            if (searchCustomer != null || searchOrderNumber != null)
            {
                page = 1;
            }
            else
            {
                searchOrderNumber = currentFilterOrderNumber;
                searchCustomer = currentFilterCustomer;
                searchDelivery = currentFilterDelivery;
                searchNonActive = currentFilterNonActive;
            }
            ViewBag.CurrentFilterQuotation = searchOrderNumber;
            ViewBag.CurrentFilterCustomer = searchCustomer;
            ViewBag.CurrentFilterDelivery = searchDelivery;
            ViewBag.CurrentFilterNonActive = searchNonActive;


            if (!String.IsNullOrEmpty(searchOrderNumber))
            {
                orders = orders.Where(o => o.OrderNumber.ToString().Contains(searchOrderNumber));
            }
            if (!String.IsNullOrEmpty(searchCustomer))
            {
                orders = orders.Where(o => o.LastName.ToUpper().Contains(searchCustomer.ToUpper()) || o.FirstName.ToUpper().Contains(searchCustomer.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchDelivery))
            {
                orders = orders.Where(o => o.customerDeliveryAddress.DeliveryAddressInfo.ToUpper().Contains(searchDelivery.ToUpper()) || o.customerDeliveryAddress.StreetName.ToUpper().Contains(searchDelivery.ToUpper()) || o.customerDeliveryAddress.Town.ToUpper().Contains(searchDelivery.ToUpper()));
            }


            switch (sortOrder)
            {
                case "order_desc":
                    orders = orders.OrderByDescending(o => o.OrderNumber);
                    break;
                case "cust_desc":
                    orders = orders.OrderByDescending(o => o.LastName);
                    break;
                case "cust":
                    orders = orders.OrderBy(o => o.LastName);
                    break;
                default:
                    orders = orders.OrderBy(o => o.OrderNumber);
                    break;
            }

            var userDefinedInfo = db.UserDefinedSettings.Find(1);
            int pageSize = userDefinedInfo.IndexResultLength;
            int pageNumber = (page ?? 1);

            if (searchNonActive == false || searchNonActive == null)
            {
                orders = orders.Where(o => o.Active.Equals(true));
            }


            //ViewBag.Quotations = quotations.ToPagedList(pageNumber, pageSize);
            oivm.orders = orders.ToPagedList(pageNumber, pageSize);

            return View(oivm);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderNumber,TotalPrice,Date,Annotation,Active,CustomerId,LastName,FirstName,CellPhone,Email,StreetName,StreetNumber,Box,PostalCodeNumber,Town")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,OrderNumber,TotalPrice,Date,Annotation,Active,CustomerId,LastName,FirstName,CellPhone,Email,StreetName,StreetNumber,Box,PostalCodeNumber,Town")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
