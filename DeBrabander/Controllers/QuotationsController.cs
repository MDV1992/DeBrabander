﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using DeBrabander.DAL;
using DeBrabander.Models;
using DeBrabander.ViewModels.Quotations;
using DeBrabander.ViewModels;
using CrystalDecisions.CrystalReports.Engine;
using PagedList;
using System.IO;

namespace DeBrabander.Controllers
{
    public class QuotationBinderCreate : IModelBinder
    {
        //private Context db2 = new Context();
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;            
            QuotationCreateViewModel obj = new QuotationCreateViewModel();
            obj.quotation.Annotation = objContext.Request.Form["QuotationInfo"];
            return obj;
        }
    }


    //ActionNameSelectorAttribute
    public class SubmitButton : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var value = controllerContext.Controller.ValueProvider.GetValue(Name);
            if (value == null)
            {
                return false;
            }
            return true;
        }
    }

    public class QuotationsController : Controller
    {
        private Context db = new Context();

        // GET: Quotations
        public ActionResult Index(string searchQuotationNumber, string currentFilterQuotationNumber ,string searchCustomer, string currentFilterCustomer,string searchDelivery, string currentFilterDelivery, bool? searchNonActive, bool? currentFilterNonActive, int? page, string sortOrder)
        {
            // viewmodel aanmaken + vullen tijdelijke lijst
            if (searchNonActive == null) { searchNonActive = false; }
            if (currentFilterNonActive == null) { currentFilterNonActive = false; }
            QuotationIndexViewModel qivm = new QuotationIndexViewModel();
            var quotations = from q in db.Quotations select q;
            Quotation quot = new Quotation();


            ViewBag.Error =  TempData["error"];
            //ViewBag.CurrentSort = sortOrder;


            ViewBag.QuotationSortParm = String.IsNullOrEmpty(sortOrder) ? "quot_desc" : "";
            ViewBag.CustomerSortParm = sortOrder=="cust" ? "cust_desc" : "cust";


            if (searchCustomer != null || searchQuotationNumber !=null)
            {
                page = 1;
            }
            else
            {
                searchQuotationNumber = currentFilterQuotationNumber;
                searchCustomer = currentFilterCustomer;
                searchDelivery = currentFilterDelivery;
                searchNonActive = currentFilterNonActive;
            }
            ViewBag.CurrentFilterQuotation = searchQuotationNumber;
            ViewBag.CurrentFilterCustomer = searchCustomer;
            ViewBag.CurrentFilterDelivery = searchDelivery;
            ViewBag.CurrentFilterNonActive = searchNonActive;


            if (!String.IsNullOrEmpty(searchQuotationNumber))
            {
                quotations = quotations.Where(q => q.QuotationNumber.ToString().Contains(searchQuotationNumber));
            }
            if (!String.IsNullOrEmpty(searchCustomer))
            {
                quotations = quotations.Where(q => q.LastName.ToUpper().Contains(searchCustomer.ToUpper()) || q.FirstName.ToUpper().Contains(searchCustomer.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchDelivery))
            {
                quotations = quotations.Where(q => q.customerDeliveryAddress.DeliveryAddressInfo.ToUpper().Contains(searchDelivery.ToUpper()) || q.customerDeliveryAddress.StreetName.ToUpper().Contains(searchDelivery.ToUpper()) || q.customerDeliveryAddress.Town.ToUpper().Contains(searchDelivery.ToUpper()));
            }

           
            switch(sortOrder)
            {
                case "quot_desc":
                    quotations = quotations.OrderByDescending(s => s.QuotationNumber);
                    break;
                case "cust_desc":
                    quotations = quotations.OrderByDescending(s => s.LastName);
                    break;
                case "cust":
                    quotations = quotations.OrderBy(s => s.LastName);
                    break;
                default:
                    quotations = quotations.OrderBy(s => s.QuotationNumber);
                    break;
            }

            var userDefinedInfo = db.UserDefinedSettings.Find(1);
            int pageSize = userDefinedInfo.IndexResultLength;
            int pageNumber = (page ?? 1);

            if (searchNonActive == false ||searchNonActive == null)
            {
                quotations = quotations.Where(q => q.Active.Equals(true));
            }


            //ViewBag.Quotations = quotations.ToPagedList(pageNumber, pageSize);
            qivm.quotations = quotations.ToPagedList(pageNumber, pageSize);

            return View(qivm);
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
        public ActionResult Create(string sortOrder, string searchStringName, string searchStringTown, string currentFilterName, string currentFilterTown, int? page)
        {
            // quotation viewmodel + ipaged list users aanmaken + nieuwe quotation voor default stuff
            QuotationCreateViewModel qcvm = new QuotationCreateViewModel();
            var customerList = from a in db.Customers select a;
            Quotation quotation = new Quotation();

            DefaultQuotationInfo(quotation);
               
            qcvm.quotation = quotation;


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

            qcvm.customers = customerList.ToPagedList(pageNumber, pageSize);


            return View(qcvm);
        }



        // POST: Quotations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SubmitButton(Name = "Create")]
        public ActionResult Create([ModelBinder(typeof(QuotationBinderCreate))]  QuotationCreateViewModel qcvm, string[] DeliveryID)
        {
            if (ModelState.IsValid)
            {
                //aanmaken van quotation / customerdeliveryaddress en customer
                Quotation quotation = new Quotation();
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
                DefaultQuotationInfo(quotation);
                quotation.FirstName = cus.FirstName;
                quotation.LastName = cus.LastName;
                quotation.Box = cus.Address.Box;
                quotation.CellPhone = cus.CellPhone;
                quotation.CustomerId = cus.CustomerId;
                quotation.Email = cus.Email;
                quotation.PostalCodeNumber = cus.Address.PostalCodeNumber;
                quotation.StreetName = cus.Address.StreetName;
                quotation.StreetNumber = cus.Address.StreetNumber;
                quotation.Town = cus.Address.Town;                
                quotation.Annotation = qcvm.quotation.Annotation;
                quotation.customerDeliveryAddress = db.CustomerDeliveryAddresses.Find(DeliveryId);

                //toevoegen en saven
                db.Quotations.Add(quotation);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(qcvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SubmitButton(Name = "AddProducts")]
        public ActionResult CreateAndAddProducts([ModelBinder(typeof(QuotationBinderCreate))]  QuotationCreateViewModel qcvm, string[] DeliveryID)
        {
            if (ModelState.IsValid)
            {
                //aanmaken van quotation / customerdeliveryaddress en customer
                Quotation quotation = new Quotation();
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
                DefaultQuotationInfo(quotation);
                quotation.FirstName = cus.FirstName;
                quotation.LastName = cus.LastName;
                quotation.Box = cus.Address.Box;
                quotation.CellPhone = cus.CellPhone;
                quotation.CustomerId = cus.CustomerId;
                quotation.Email = cus.Email;
                quotation.PostalCodeNumber = cus.Address.PostalCodeNumber;
                quotation.StreetName = cus.Address.StreetName;
                quotation.StreetNumber = cus.Address.StreetNumber;
                quotation.Town = cus.Address.Town;
                quotation.Annotation = qcvm.quotation.Annotation;
                quotation.customerDeliveryAddress = db.CustomerDeliveryAddresses.Find(DeliveryId);

                //toevoegen en saven
                db.Quotations.Add(quotation);
                db.SaveChanges();

                int id = quotation.QuotationId;
                TempData["id"] = id;
                return RedirectToAction("AddProducts");

            }
            return View(qcvm);
        }

        private Quotation DefaultQuotationInfo(Quotation quotation)
        {
            //basis info invullen in quotation
            //ophalen van lijst quotations voor vinden van laatste quotationnummer en dan +1 
            var listquotations = new List<Quotation>();
            listquotations = db.Quotations.ToList();
            var userSettings = db.UserDefinedSettings.Find(1);
            int expirationDateLengt = userSettings.ExpirationDateLength;

            int maxQuotationnumber = 1;
            quotation.QuotationNumber = maxQuotationnumber;
            if (listquotations.Count != 0)
            {
                maxQuotationnumber = listquotations.Max(r => r.QuotationNumber);
                quotation.QuotationNumber = maxQuotationnumber + 1;
            }
            quotation.Active = true;
            quotation.Date = DateTime.Now;
            quotation.ExpirationDate = quotation.Date.AddMonths(expirationDateLengt);

            return quotation;
        }


        public ActionResult AddProducts(int? id,int? page, string searchString, string currentFilterSearchString, string categoryId, string currentFilterCategoryId, string sortOrder)
        {
            if (id == null)
            {
                id = (int)TempData["id"];
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quotation quotation = new Quotation();
            QuotationAddProductsViewModel qapvm = new QuotationAddProductsViewModel();
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
            
            switch(sortOrder)
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
            qapvm.products = productList.ToPagedList(pageNumber, pageSize);


            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryID", "CategoryName", categoryId);
            

            
            quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            qapvm.quotation = quotation;
            CalculateTotalPriceinc(id);

            return View("AddProducts", qapvm);
        }




        // GET: Quotations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // opzoeken van de juiste quotation
            Quotation quotation = db.Quotations.Find(id);
            // alle klantgegevens ophalen van de offerte
            var customer = db.Customers.Find(quotation.CustomerId);
            // info opzoeken van het werfadres
            Address werfadres = new Address();
            werfadres.Box = quotation.customerDeliveryAddress.Box;
            werfadres.PostalCodeNumber = quotation.customerDeliveryAddress.PostalCodeNumber;
            werfadres.StreetName = quotation.customerDeliveryAddress.StreetName;
            werfadres.StreetNumber = quotation.customerDeliveryAddress.StreetNumber;
            werfadres.Town = quotation.customerDeliveryAddress.Town;

            // vullen van viewmodel
            QuotationEditViewModel qevm = new QuotationEditViewModel();
            qevm.quotation = quotation;
            qevm.customer = customer;
            qevm.address = werfadres;



            if (quotation == null)
            {
                return HttpNotFound();
            }
            // omgezet naar model
            //ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FullName");
            //var quotDetList = from q in db.QuotationDetails select q;
            //quotDetList = quotDetList.Where(q => q.QuotationId == id);
            //ViewBag.QuotationDetail = quotDetList.ToList();
            return View(qevm);
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


        public ActionResult AddProductToQuotation(int? quotationId, int? productId)
        {
            // find op quotation en product
            var quotItem = db.QuotationDetails.SingleOrDefault(q => q.QuotationId == quotationId && q.ProductId == productId);
            Product prod = db.Products.Find(productId);
            
            // nieuwe quotation detail
            if (quotItem == null)
            {
                quotItem = new QuotationDetail
                {
                    ProductId = (int)productId,
                    QuotationId = (int)quotationId,
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
                quotItem.VAT = db.VATs.Find(quotItem.VATPercId);
                db.QuotationDetails.Add(quotItem);
            }
            else
            {
                quotItem.Quantity++;
            }
            quotItem.TotalExVat = (quotItem.PriceExVAT + quotItem.Auvibel + quotItem.Bebat + quotItem.Recupel + quotItem.Reprobel) * quotItem.Quantity;
            //Berekent totaal per lijn van producten inc BTW
            quotItem.TotalIncVat = quotItem.TotalExVat * (1 + (quotItem.VAT.VATValue / 100));
            CalculateTotalPriceinc(quotationId);
            db.SaveChanges();

            return RedirectToAction("AddProducts", new { id= quotationId });
        }


        public ActionResult CopyQuotation(int? Id)
        {
            Quotation quotNew = new Quotation();
            Quotation quotOld = new Quotation();
            quotOld = db.Quotations.Find(Id);

            //check of er al een copy is
            List<Quotation> quotlist = db.Quotations.ToList();
            var checkQuotNumber  = quotlist.Where(q => q.QuotationNumber == quotOld.QuotationNumber);
            

            if (checkQuotNumber.Count() == 1)
            {
                quotNew.Annotation = quotOld.Annotation + " - Copy";
                quotNew.Active = quotOld.Active;
                quotNew.Box = quotOld.Box;
                quotNew.CellPhone = quotOld.CellPhone;
                quotNew.CustomerId = quotOld.CustomerId;
                quotNew.Date = quotOld.Date;
                quotNew.Email = quotOld.Email;
                quotNew.ExpirationDate = quotOld.ExpirationDate;
                quotNew.FirstName = quotOld.FirstName;
                quotNew.LastName = quotOld.LastName;
                quotNew.PostalCodeNumber = quotOld.PostalCodeNumber;
                quotNew.QuotationNumber = quotOld.QuotationNumber;
                quotNew.StreetName = quotOld.StreetName;
                quotNew.StreetNumber = quotOld.StreetNumber;
                quotNew.TotalPrice = 0;
                quotNew.Town = quotOld.Town;

                db.Quotations.Add(quotNew);
                db.SaveChanges();

                var delivery = db.CustomerDeliveryAddresses.Find(quotOld.customerDeliveryAddress.CustomerDeliveryAddressId);
                quotNew.customerDeliveryAddress = delivery;

                foreach (var item in quotOld.QuotationDetail)
                {

                    var qd = new QuotationDetail();
                    qd.Quantity = item.Quantity;
                    qd.PriceExVAT = item.PriceExVAT;
                    qd.TotalExVat = item.TotalExVat;
                    qd.TotalIncVat = item.TotalIncVat;
                    qd.Auvibel = item.Auvibel;
                    qd.Bebat = item.Bebat;
                    qd.Brand = item.Brand;
                    qd.CategoryId = item.CategoryId;
                    qd.Description = item.Description;
                    qd.ProductCode = item.ProductCode;
                    qd.ProductName = item.ProductName;
                    qd.Recupel = item.Recupel;
                    qd.Reprobel = item.Reprobel;
                    qd.VATPercId = item.VATPercId;
                    qd.ProductId = item.ProductId;
                    qd.QuotationId = quotNew.QuotationId;
                    qd.VAT = item.VAT;
                    db.QuotationDetails.Add(qd);

                }

                db.SaveChanges();
            }
            else
            {
                TempData["error"] = "Er is reeds een copy van deze offerte";
            }
            return RedirectToAction("index");
        }

        public ActionResult CreateOrder(int? Id)
        {
            Order order = new Order();
            Quotation quot = new Quotation();
            quot = db.Quotations.Find(Id);
            
            order.Annotation = quot.Annotation + " - Order van Offerte " + quot.QuotationNumber;
            order.Box = quot.Box;
            order.CellPhone = quot.CellPhone;
            order.CustomerId = quot.CustomerId;
            order.Date = quot.Date;
            order.Email = quot.Email;
            order.FirstName = quot.FirstName;
            order.LastName = quot.LastName;
            order.PostalCodeNumber = quot.PostalCodeNumber;
            order.StreetName = quot.StreetName;
            order.StreetNumber = quot.StreetNumber;
            order.TotalPrice = quot.TotalPrice;
            order.Town = quot.Town;

            db.Orders.Add(order);
            db.SaveChanges();

            

            order.customerDeliveryAddress = quot.customerDeliveryAddress;

            //find highest order number
            int maxOrderNumber = 1;
            order.OrderNumber = maxOrderNumber;
            var listOrders = db.Orders.ToList();

            if (listOrders.Count != 0)
            {
                maxOrderNumber = listOrders.Max(o => o.OrderNumber);
                order.OrderNumber = maxOrderNumber + 1;
            }


            foreach (var item in quot.QuotationDetail)
            {
                var od = new OrderDetail();
                od.Quantity = item.Quantity;
                od.PriceExVAT = item.PriceExVAT;
                od.TotalExVat = item.TotalExVat;
                od.TotalIncVat = item.TotalIncVat;
                od.Auvibel = item.Auvibel;
                od.Bebat = item.Bebat;
                od.Brand = item.Brand;
                od.CategoryId = item.CategoryId;
                od.Description = item.Description;
                od.ProductCode = item.ProductCode;
                od.ProductName = item.ProductName;
                od.Recupel = item.Recupel;
                od.Reprobel = item.Reprobel;
                od.VATPercId = item.VATPercId;
                od.ProductId = item.ProductId;
                od.VAT = item.VAT;
                db.OrderDetails.Add(od);
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Orders");
        }

        //Methode voor berekenen totale prijs offerte
        public void CalculateTotalPriceinc(int? quotationId)
        {
            double totalPriceQuotation = 0;
            Quotation quot = db.Quotations.Find(quotationId);
            foreach (var qd in quot.QuotationDetail)
            {
                totalPriceQuotation = quot.QuotationDetail.Sum(x => x.TotalIncVat);
            }
            quot.TotalPrice = totalPriceQuotation;
            db.SaveChanges();

        }


        public ActionResult CRAllQuotations(int? id)
        {
            List<Quotation> allQuotations = new List<Quotation>();
            allQuotations = db.Quotations.ToList();

            List<Company> company = new List<Company>();
            company = db.Companies.ToList();

            List<AllQuotationsCRViewModel> allQuotationsCR = new List<AllQuotationsCRViewModel>();
            foreach (var item in allQuotations)
            {
                var allQuotationCR = new AllQuotationsCRViewModel();
                allQuotationCR.QuotationId = item.QuotationId;
                allQuotationCR.QuotationNumber = item.QuotationNumber;
                allQuotationCR.Active = item.Active;
                allQuotationCR.ExpirationDate = item.ExpirationDate;
                allQuotationCR.Date = item.Date;
                allQuotationCR.CustomerId = item.CustomerId;
                allQuotationCR.Annotation = item.Annotation;
                allQuotationCR.FirstName = item.FirstName;
                allQuotationCR.LastName = item.LastName;
                allQuotationCR.StreetName = item.StreetName;
                allQuotationCR.StreetNumber = item.StreetNumber;
                allQuotationCR.Town = item.Town;
                allQuotationCR.PostalCodeNumber = item.PostalCodeNumber;
                allQuotationCR.Box = item.Box;
                allQuotationCR.TotalPrice = item.TotalPrice;

                allQuotationsCR.Add(allQuotationCR);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "AllQuotationsMain.rpt"));
            rd.OpenSubreport("Header.rpt").SetDataSource(company);
            rd.OpenSubreport("allQuotationsSub.rpt").SetDataSource(allQuotationsCR);
            //rd.SetDataSource(company);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "All_Quotations.pdf");
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

        public ActionResult CRQuotation(int id)
        {
            
            //Lijsten maken en vullen
            List<SingleQuotationCRViewModel> CRSingleQuotationHeaderListVM = new List<SingleQuotationCRViewModel>();
            SingleQuotationCRViewModel CRSQH = new SingleQuotationCRViewModel();

            Quotation quotation = db.Quotations.Find(id);
            Company company = db.Companies.Find(1);
            Customer cus = db.Customers.Find(quotation.CustomerId);

            
            //vulling Header info
            //company
            CRSQH.BIC = company.BIC;
            CRSQH.CompanyId = company.CompanyId;
            CRSQH.CompanyName = company.CompanyName;
            CRSQH.Country = company.Country;
            CRSQH.District = company.District;
            CRSQH.EmailCompany = company.Email;
            CRSQH.Iban = company.Iban;
            CRSQH.Mobile = company.Mobile;
            CRSQH.Phone = company.Phone;
            CRSQH.Postalcode = company.Postalcode;
            CRSQH.Street = company.Street;
            CRSQH.VatNumber = company.VatNumber;
            CRSQH.Website = company.Website;

            //quotation
            CRSQH.Annotation = quotation.Annotation;
            CRSQH.Box = quotation.Box;
            CRSQH.CellPhone = quotation.CellPhone;
            CRSQH.CustomerId = quotation.CustomerId;
            CRSQH.Date = quotation.Date;
            CRSQH.EmailCustomer = quotation.Email;
            CRSQH.ExpirationDate = quotation.ExpirationDate;
            CRSQH.FirstName = quotation.FirstName;
            CRSQH.LastName = quotation.LastName;
            CRSQH.PostalCodeNumber = quotation.PostalCodeNumber;
            CRSQH.QuotationId = quotation.QuotationId;
            CRSQH.QuotationNumber = quotation.QuotationNumber;
            CRSQH.StreetName = quotation.StreetName;
            CRSQH.StreetNumber = quotation.StreetNumber;
            CRSQH.TotalPrice = quotation.TotalPrice;
            CRSQH.Town = quotation.Town;
            CRSQH.VATnumberCustomer = cus.VATNumber;

            //customer contact info
            CRSQH.ContactCellPhone = cus.ContactCellPhone;
            CRSQH.ContactEmail = cus.ContactEmail;
            CRSQH.ContactName = cus.ContactName;
            
            //delivery info
            CRSQH.DeliveryAddressInfo = quotation.customerDeliveryAddress.DeliveryAddressInfo;
            CRSQH.PostalCodeNumberTown = quotation.customerDeliveryAddress.PostalCodeNumber + " " + quotation.customerDeliveryAddress.Town;
            CRSQH.StreetNameNumberBox = quotation.customerDeliveryAddress.StreetName + " " + quotation.customerDeliveryAddress.StreetNumber + " " + quotation.customerDeliveryAddress.Box;

            //vulling details info
            foreach (var item in quotation.QuotationDetail)
            {
                CRSQH.ProductCode = item.ProductCode;
                CRSQH.PriceExVAT= item.PriceExVAT;
                CRSQH.Quantity = item.Quantity;
                CRSQH.Description = item.Description;
                CRSQH.VATValue = Convert.ToInt16(item.VAT.VATValue);
                CRSQH.Auvibel = item.Auvibel;
                CRSQH.Recupel = item.Recupel;
                CRSQH.Reprobel = item.Reprobel;
                CRSQH.Bebat = item.Bebat;
                CRSQH.ProductCode = item.ProductCode;
                CRSQH.ProductName = item.ProductName;
                CRSingleQuotationHeaderListVM.Add(CRSQH);
                SingleQuotationCRViewModel empty = new SingleQuotationCRViewModel();
                CRSQH = empty;
            }
            
            

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/Quotation"), "SingleQuotationHeader.rpt"));
            rd.SetDataSource(CRSingleQuotationHeaderListVM);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                string pdfName = "Offerte - "+quotation.QuotationNumber+ " - "  + quotation.FullName.ToString()+".pdf";
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
