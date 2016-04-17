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
    public class OrderBinderCreate : IModelBinder
    {
        //private Context db2 = new Context();
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;
            OrderCreateViewModel obj = new OrderCreateViewModel();
            obj.order.Annotation = objContext.Request.Form["OrderInfo"];
            return obj;
        }
    }
    
    
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
        public ActionResult Create(string sortOrder, string searchStringName, string searchStringTown, string currentFilterName, string currentFilterTown, int? page)
        {
            // quotation viewmodel + ipaged list users aanmaken + nieuwe quotation voor default stuff
            OrderCreateViewModel ocvm = new OrderCreateViewModel();
            var customerList = from a in db.Customers select a;
            Order order = new Order();

            DefaultOrderInfo(order);

            ocvm.order = order;


            //zoeken / sorteren / paging
            //sorteren  default op "name_desc" 
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TownSortParm = sortOrder == "town" ? "town_desc" : "town";


            // als zoeken leeg is pagina 1 anders 
            if (searchStringTown != null || searchStringName != null)
            {
                page = 1;
            }
            else
            {
                searchStringName = currentFilterName;
                searchStringTown = currentFilterTown;
            }
            ViewBag.CurrentFilterName = searchStringName;
            ViewBag.CurrentFilterTown = searchStringTown;



            // zoekvelden toepassen op klantenlijst
            //zoeken op naam (voor of achter en/of gemeente)           
            if (!String.IsNullOrEmpty(searchStringTown))
            {
                customerList = customerList.Where(s => s.Address.Town.ToUpper().Contains(searchStringTown.ToUpper()));
            }

            // zoeken op postalcode
            if (!String.IsNullOrEmpty(searchStringName))
            {
                customerList = customerList.Where(s => s.LastName.ToUpper().Contains(searchStringName.ToUpper()) ||
                s.FirstName.ToUpper().Contains(searchStringName.ToUpper()));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    customerList = customerList.OrderByDescending(s => s.LastName);
                    break;
                case "town":
                    customerList = customerList.OrderBy(s => s.Address.Town);
                    break;
                case "town_desc":
                    customerList = customerList.OrderByDescending(s => s.Address.Town);
                    break;
                default:
                    customerList = customerList.OrderBy(s => s.LastName);
                    break;
            }

            var userDefinedInfo = db.UserDefinedSettings.Find(1);
            int pageSize = userDefinedInfo.DetailsResultLength;
            int pageNumber = (page ?? 1);

            ocvm.customers = customerList.ToPagedList(pageNumber, pageSize);


            return View(ocvm);
        }

        private Order DefaultOrderInfo(Order order)
        {
            //basis info invullen in order
            //ophalen van lijst orders voor vinden van laatste ordernummer en dan +1 
            var listOrders = new List<Order>();
            listOrders = db.Orders.ToList();
            var userSettings = db.UserDefinedSettings.Find(1);
            

            int maxOrdernumber = 1;
            order.OrderNumber = maxOrdernumber;
            if (listOrders.Count != 0)
            {
                maxOrdernumber = listOrders.Max(o => o.OrderNumber);
                order.OrderNumber = maxOrdernumber + 1;
            }
            order.Active = true;
            order.Date = DateTime.Now;            

            return order;
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SubmitButton(Name ="CreateOrder")]
        public ActionResult Create([ModelBinder(typeof(QuotationBinderCreate))]  OrderCreateViewModel ocvm, string[] DeliveryID)
        {
            if (ModelState.IsValid)
            {
                //aanmaken van quotation / customerdeliveryaddress en customer
                Order order  = new Order();
                CustomerDeliveryAddress cda = new CustomerDeliveryAddress();
                Customer cus = new Customer();

                //ophalen van customerDeliveryAddressID vanuit de array
                // eventueel nog test inbouwen voor lengte van array (check dat ze niet meer als 1 veld selecteren)
                int DeliveryId = 1;
                if (DeliveryID.Length == 1)
                {
                    DeliveryId = Int32.Parse(DeliveryID.First());
                }
                else
                {
                    return RedirectToAction("Create");
                }

                // ophalen delivery info en van daaruit customer info
                cda = db.CustomerDeliveryAddresses.Find(DeliveryId);
                cus = db.Customers.Find(cda.CustomerId);

                // invullen van de klant info in de quotation
                DefaultOrderInfo(order);
                order.FirstName = cus.FirstName;
                order.LastName = cus.LastName;
                order.Box = cus.Address.Box;
                order.CellPhone = cus.CellPhone;
                order.CustomerId = cus.CustomerId;
                order.Email = cus.Email;
                order.PostalCodeNumber = cus.Address.PostalCodeNumber;
                order.StreetName = cus.Address.StreetName;
                order.StreetNumber = cus.Address.StreetNumber;
                order.Town = cus.Address.Town;
                order.Annotation = ocvm.order.Annotation;
                order.customerDeliveryAddress = db.CustomerDeliveryAddresses.Find(DeliveryId);

                //toevoegen en saven
                db.Orders.Add(order);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(ocvm);
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
