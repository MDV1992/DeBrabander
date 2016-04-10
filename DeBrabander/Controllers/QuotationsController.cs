using System;
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
using PagedList;

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
        public ActionResult Index(string searchQuotationNumber, string currentFilterQuotationNumber ,string searchCustomer, string currentFilterCustomer,string searchDelivery, string currentFilterDelivery, int? page, string sortOrder)
        {
            // viewmodel aanmaken + vullen tijdelijke lijst
            QuotationIndexViewModel qivm = new QuotationIndexViewModel();
            var quotations = from q in db.Quotations select q;
            Quotation quot = new Quotation();

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
            }
            ViewBag.CurrentFilterQuotation = searchQuotationNumber;
            ViewBag.CurrentFilterCustomer = searchCustomer;
            ViewBag.CurrentFilterDelivery = searchDelivery;

            
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
            quotItem.TotalExVat = quotItem.PriceExVAT * quotItem.Quantity;
            //Berekent totaal per lijn van producten inc BTW
            quotItem.TotalIncVat = quotItem.TotalExVat * (1 + (quotItem.VAT.VATValue / 100));
            CalculateTotalPriceinc(quotationId);
            db.SaveChanges();

            return RedirectToAction("AddProducts", new { id= quotationId });
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
