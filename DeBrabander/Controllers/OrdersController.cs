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
using DeBrabander.ViewModels;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
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
        public ActionResult Create([ModelBinder(typeof(OrderBinderCreate))]  OrderCreateViewModel ocvm, string[] DeliveryID)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SubmitButton(Name = "AddProductsOrder")]
        public ActionResult CreateAndAddProducts([ModelBinder(typeof(OrderBinderCreate))]  OrderCreateViewModel ocvm, string[] DeliveryID)
        {
            if (ModelState.IsValid)
            {
                //aanmaken van quotation / customerdeliveryaddress en customer
                Order order = new Order();
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

                int id = order.OrderId;
                TempData["id"] = id;
                return RedirectToAction("AddProducts");

            }
            return View(ocvm);
        }

        public ActionResult AddProducts(int? id, int? page, string searchString, string currentFilterSearchString, string categoryId, string currentFilterCategoryId, string sortOrder)
        {
            if (id == null)
            {
                id = (int)TempData["id"];
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = new Order();
            OrderAddProductsViewModel oapvm = new OrderAddProductsViewModel();
            var productList = from p in db.Products select p;
            productList = productList.Where(p => p.Active.Equals(true));

            ViewBag.ProductSortParm = string.IsNullOrEmpty(sortOrder) ? "prod_desc" : "";


            //aanmaken product list + filtering


            //paging
            if (searchString != null || categoryId != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilterSearchString;
                categoryId = currentFilterCategoryId;
            }
            ViewBag.CurrentFilterSearchString = searchString;
            ViewBag.CurrentFilterCategoryId = categoryId;


            // Zoekfunctie
            if (!String.IsNullOrEmpty(searchString))
            {
                productList = productList.Where(x => x.ProductName.ToUpper().Contains(searchString.ToUpper()) || x.ProductCode.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(categoryId))
            {
                int catId = int.Parse(categoryId);
                productList = productList.Where(x => x.CategoryId == catId);
            }

            switch (sortOrder)
            {
                case "prod_dec":
                    productList = productList.OrderByDescending(p => p.ProductName);
                    break;
                default:
                    productList = productList.OrderBy(p => p.ProductName);
                    break;
            }


            var userDefinedInfo = db.UserDefinedSettings.Find(1);
            int pageSize = userDefinedInfo.DetailsResultLength;
            int pageNumber = (page ?? 1);
            oapvm.products = productList.ToPagedList(pageNumber, pageSize);


            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", categoryId);



            order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            oapvm.order = order;
            CalculateTotalPriceinc(id);

            return View("AddProducts", oapvm);
        }
        public ActionResult AddProductToOrder(int? orderId, int? productId)
        {
            // find op order en product
            var orderItem = db.OrderDetails.SingleOrDefault(o => o.OrderId == orderId && o.ProductId == productId);
            Product prod = db.Products.Find(productId);

            // nieuwe quotation detail
            if (orderItem == null)
            {
                orderItem = new OrderDetail
                {
                    ProductId = (int)productId,
                    OrderId = (int)orderId,
                    Quantity = 1,
                    Auvibel = prod.Auvibel,
                    Bebat = prod.Bebat,
                    CategoryId = prod.CategoryId,
                    Brand = prod.Brand,
                    Description = prod.Description,
                    PriceExVAT = prod.PriceExVAT,
                    ProductCode = prod.ProductCode,
                    ProductName = prod.ProductName,
                    Recupel = prod.Recupel,
                    Reprobel = prod.Reprobel,
                    VATPercId = prod.VATPercId,
                };
                orderItem.VAT = db.VATs.Find(orderItem.VATPercId);
                db.OrderDetails.Add(orderItem);
            }
            else
            {
                orderItem.Quantity++;
            }
            orderItem.TotalExVat = (orderItem.PriceExVAT + orderItem.Auvibel + orderItem.Bebat + orderItem.Recupel + orderItem.Reprobel) * orderItem.Quantity;
            //Berekent totaal per lijn van producten inc BTW
            orderItem.TotalIncVat = orderItem.TotalExVat * (1 + (orderItem.VAT.VATValue / 100));
            CalculateTotalPriceinc(orderId);
            db.SaveChanges();

            return RedirectToAction("AddProducts", new { id = orderId });
        }

        private void CalculateTotalPriceinc(int? orderId)
        {
            
            double totalPriceOrder = 0;
            Order order = db.Orders.Find(orderId);
            foreach (var od in order.OrderDetail)
            {
                totalPriceOrder = order.OrderDetail.Sum(x => x.TotalIncVat);
            }
            order.TotalPrice = totalPriceOrder;
            db.SaveChanges();

        }
    

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            var customer = db.Customers.Find(order.CustomerId);
            Address werfadres = new Address();
            werfadres.Box = order.customerDeliveryAddress.Box;
            werfadres.PostalCodeNumber = order.customerDeliveryAddress.PostalCodeNumber;
            werfadres.StreetName = order.customerDeliveryAddress.StreetName;
            werfadres.StreetNumber = order.customerDeliveryAddress.StreetNumber;
            werfadres.Town = order.customerDeliveryAddress.Town;

            OrderEditViewModel oevm = new OrderEditViewModel();
            oevm.order = order;
            oevm.customer = customer;
            oevm.address = werfadres;

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(oevm);
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

        public ActionResult CRAllOrders(int? id)
        {
            var allOrdersvar = from a in db.Orders select a;
            allOrdersvar = allOrdersvar.Where(o => o.Active.Equals(true));

            List<Order> ActiveOrderlist = allOrdersvar.ToList();

            List<Company> company = new List<Company>();
            company = db.Companies.ToList();

            List<AllOrdersCRViewModel> allOrdersCR = new List<AllOrdersCRViewModel>();
            foreach (var item in ActiveOrderlist)
            {
                    var allOrderCR = new AllOrdersCRViewModel();
                    allOrderCR.OrderId = item.OrderId;
                    allOrderCR.OrderNumber = item.OrderNumber;
                    allOrderCR.Active = item.Active;
                    allOrderCR.Date = item.Date;
                    allOrderCR.CustomerId = item.CustomerId;
                    allOrderCR.Annotation = item.Annotation;
                    allOrderCR.FirstName = item.FirstName;
                    allOrderCR.LastName = item.LastName;
                    allOrderCR.Email = item.Email;
                    allOrderCR.StreetName = item.StreetName;
                    allOrderCR.StreetNumber = item.StreetNumber;
                    allOrderCR.Box = item.Box;
                    allOrderCR.CellPhone = item.CellPhone;
                    allOrderCR.PostalCodeNumber = item.PostalCodeNumber;
                    allOrderCR.Town = item.Town;
                    allOrderCR.TotalPrice = item.TotalPrice;

                    allOrdersCR.Add(allOrderCR);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "AllQuotationsMain.rpt"));
            rd.OpenSubreport("Header.rpt").SetDataSource(company);
            rd.OpenSubreport("allQuotationsSub.rpt").SetDataSource(allOrdersCR);
            //rd.SetDataSource(company);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "All_Orders.pdf");
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
