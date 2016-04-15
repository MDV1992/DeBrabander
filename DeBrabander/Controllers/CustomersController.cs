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
using DeBrabander.ViewModels;
using PagedList;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

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
            //obj.customer.TAXLiability = objContext.Request.Form["customer.TAXLiability"];
            

            obj.customer.Address.Town = objContext.Request.Form["customer.Address.Town"];
            obj.customer.Address.PostalCodeNumber = int.Parse(objContext.Request.Form["customer.Address.PostalCodeNumber"]);
            obj.customer.Address.StreetName = objContext.Request.Form["customer.Address.StreetName"];
            obj.customer.Address.StreetNumber = int.Parse(objContext.Request.Form["customer.Address.StreetNumber"]);
            obj.customer.Address.Box = int.Parse(objContext.Request.Form["customer.Address.Box"]);        
            

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
            obj.customer.Address.PostalCodeNumber = int.Parse(objContext.Request.Form["customer.Address.PostalCodeNumber"]);
            obj.customer.Address.Town = objContext.Request.Form["customer.Address.Town"];


            return obj;

        }
    }

    public class CustomerBinderAddNewDeliveryAddress : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpContextBase objContext = controllerContext.HttpContext;

            CustomerAddDeliveryAddressViewModel obj = new CustomerAddDeliveryAddressViewModel();
            obj.address = new Address();
            obj.deliveryAddress = new CustomerDeliveryAddress();

            obj.deliveryAddress.DeliveryAddressInfo = objContext.Request.Form["DeliveryAddressInfo"];
            obj.deliveryAddress.Box = Int32.Parse(objContext.Request.Form["Box"]);
            obj.deliveryAddress.PostalCodeNumber = Int32.Parse(objContext.Request.Form["PostalCodeNumber"]);
            obj.deliveryAddress.StreetName = objContext.Request.Form["StreetName"];
            obj.deliveryAddress.StreetNumber = Int32.Parse(objContext.Request.Form["StreetNumber"]);
            obj.deliveryAddress.Town = objContext.Request.Form["Town"];
            obj.deliveryAddress.CustomerId = Int32.Parse(objContext.Request.Form["CustomerId"]);            
            
            return obj;

        }
    }


    public class CustomersController : Controller
    {
        private Context db = new Context();

        // GET: Customers
        public ActionResult Index(string sortOrder, string searchStringName, string searchStringTown, string currentFilterName, string currentFilterTown, int? page)
        {
            // nieuw ViewModel aanmaken en tijdelijke lijst vullen met klant info
            CustomerIndexViewModel civm = new CustomerIndexViewModel();
            var customerList = from a in db.Customers select a;
            


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
            int pageSize = userDefinedInfo.IndexResultLength;
            int pageNumber = (page ?? 1);

            civm.customers = customerList.ToPagedList(pageNumber, pageSize);
            return View(civm);
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
                // aanmaken lege opbjecten 
                Customer cus = new Customer();
                CustomerDeliveryAddress cda = new CustomerDeliveryAddress();
                
                //opslaan van klant
                cus = ccvm.customer;
                db.Customers.Add(cus);
                db.SaveChanges();

                // vullen van delivery Address met default info
                cda.Box = cus.Address.Box;
                cda.PostalCodeNumber = cus.Address.PostalCodeNumber;
                cda.StreetName = cus.Address.StreetName;
                cda.StreetNumber = cus.Address.StreetNumber;
                cda.Town = cus.Address.Town;
                cda.CustomerId = cus.CustomerId;
                cda.DeliveryAddressInfo = "Standaard adres";
                db.CustomerDeliveryAddresses.Add(cda);
                db.SaveChanges();

                //redirect to index
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
        public ActionResult AddDeliveryAddress(int? id, int? page, string sortOrder, string searchStringTown, string searchStringPostal, string currentFilterTown, string currentFilterPostal)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.CurrentSort = sortOrder;

            // hij kijkt of town en postal leeg is indien niet leeg terug reset naar pagina 1
            if (searchStringTown != null || searchStringPostal != null)
            {
                page = 1;
            }
            else
            {
                searchStringTown = currentFilterTown;
                searchStringPostal = currentFilterPostal;
            }
            ViewBag.CurrentFilterTown = searchStringTown;
            ViewBag.CurrentFilterPostal = searchStringPostal;

            //lijst vullen met adressen om weer te geven
            var addressList = from a in db.Addresses select a;

            // zoekvelden toepassen op addresslist
            //zoeken op gemeente of adres           
            if (!String.IsNullOrEmpty(searchStringTown))
            {
                addressList = addressList.Where(s => s.Town.ToUpper().Contains(searchStringTown.ToUpper()) ||
                s.StreetName.ToUpper().Contains(searchStringTown.ToUpper()));
            }

            // zoeken op postalcode
            if (!String.IsNullOrEmpty(searchStringPostal))
            {
                int postalCode = int.Parse(searchStringPostal);
                addressList = addressList.Where(s => s.PostalCodeNumber == (postalCode)) ;
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
            var userDefinedInfo = db.UserDefinedSettings.Find(1);
            int pageSize = userDefinedInfo.DetailsResultLength;
            int pageNumber = (page ?? 1);

            ViewBag.Addressess = addressList.ToPagedList(pageNumber, pageSize);
            



            Customer customer = db.Customers.Find(id);
            CustomerAddDeliveryAddressViewModel cavm = new CustomerAddDeliveryAddressViewModel();
            cavm.customer = customer;
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.returnUrl = Request.UrlReferrer;
            return View("AddDeliveryAddress",cavm);
        }


        [HttpPost]
        public ActionResult AddDeliveryAddress([ModelBinder(typeof(CustomerBinderAddNewDeliveryAddress))] CustomerAddDeliveryAddressViewModel cadavm, string[] AddressId, string returnUrl)
        {
            
            Customer cus = new Customer();
            Address add = new Address();
            CustomerDeliveryAddress cda = new CustomerDeliveryAddress();
            cus = db.Customers.Find(cadavm.deliveryAddress.CustomerId);

            if (AddressId != null)
            {
                if (AddressId.Length > 1)
                {
                    return RedirectToAction("AddDeliveryAddress");
                }

                if (AddressId.Length == 1)
                {
                    add = db.Addresses.Find(Int32.Parse(AddressId.First()));
                    cda.Box = add.Box;
                    cda.CustomerId = cadavm.customer.CustomerId;
                    cda.DeliveryAddressInfo = cadavm.deliveryAddress.DeliveryAddressInfo;
                    cda.PostalCodeNumber = add.PostalCodeNumber;
                    cda.StreetName = add.StreetName;
                    cda.StreetNumber = add.StreetNumber;
                    cda.Town = add.Town;
                    cus.CustomerDeliveryAddress.Add(cda);
                    db.SaveChanges();
                    return Redirect(returnUrl);
                }
            }

            cda = cadavm.deliveryAddress;
            cus.CustomerDeliveryAddress.Add(cda);
            db.SaveChanges();
            return Redirect(returnUrl);


        }



        public ActionResult CRAllCustomers(int? id)
        {
            List<Customer> allCustomers = new List<Customer>();
            allCustomers = db.Customers.ToList();

            List<Company> company = new List<Company>();
            company = db.Companies.ToList();

            List<AllCustomersCRViewModel> allCustomersCR = new List<AllCustomersCRViewModel>();
            foreach (var item in allCustomers)
            {
                var allCustomerCR = new AllCustomersCRViewModel();
                allCustomerCR.FirstName = item.FirstName;
                allCustomerCR.LastName = item.LastName;
                allCustomerCR.CompanyName = item.CompanyName;
                allCustomerCR.Phone = item.Phone;
                allCustomerCR.CellPhone = item.CellPhone;
                allCustomerCR.VATNumber = item.VATNumber;
                allCustomerCR.ContactName = item.ContactName;
                allCustomerCR.ContactEmail = item.ContactEmail;
                allCustomerCR.ContactCellPhone = item.ContactCellPhone;
                allCustomerCR.Type = item.Type;
                allCustomerCR.TAXLiability = item.TAXLiability;
                allCustomerCR.AccountNumber = item.AccountNumber;
                allCustomerCR.Annotation = item.Annotation;
                allCustomerCR.Email = item.Email;
                allCustomerCR.CustomerId = item.CustomerId;
                allCustomerCR.AddressId = item.Address.AddressId;
                allCustomerCR.StreetName = item.Address.StreetName;
                allCustomerCR.StreetNumber = item.Address.StreetNumber;
                allCustomerCR.Town = item.Address.Town;

                allCustomersCR.Add(allCustomerCR);
            }

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "MainCustomers.rpt"));
            rd.OpenSubreport("Header.rpt").SetDataSource(company);
            rd.OpenSubreport("allCustomers.rpt").SetDataSource(allCustomersCR);
            //rd.SetDataSource(company);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "All_Customers.pdf");
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
