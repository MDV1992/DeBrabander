using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DeBrabander.Models;
using System.Data.Entity.Infrastructure;
using DeBrabander.Models.Company;

namespace DeBrabander.DAL
{
    public class Initializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            //context.Customers.Add(new Customer { LastName = "Gersis", FirstName = "Glenn" });
            //context.Customers.Add(new Customer { LastName = "Brunson", FirstName = "Tom" });
            //context.Customers.Add(new Customer { LastName = "DVS", FirstName = "Maarten" });
            //base.Seed(context);


            PostalCode postel1 = new PostalCode { PostalCodeNumber = 2900, Town = "Schoten" };
            PostalCode postel2 = new PostalCode { PostalCodeNumber = 2170, Town = "Merksem" };
            PostalCode postel3 = new PostalCode { PostalCodeNumber = 2930, Town = "Brasschaat" };


            Address add1 = new Address { StreetName = "Boekweitstraat", StreetNumber = 74, PostalCode = postel1 };
            Address add2 = new Address { StreetName = "Bredabaan", StreetNumber = 813, PostalCode = postel2 };

            var Customers = new List<Customer>
            {
                new Customer { FirstName="Tom", LastName="Brunson", Address = add1 },
                new Customer { FirstName="Maarten", LastName="DeVleesSchouwer", Address = add1 },
                new Customer { FirstName="Glenn", LastName="Gersis", Address = add2 }
            };

            foreach (var temp in Customers)
            {
                context.Customers.Add(temp);
            }
            context.SaveChanges();

            var CustomerDeliveryAddress = new List<CustomerDeliveryAddress>
            {
                new CustomerDeliveryAddress { DeliveryAddressInfo="Werf1", CustomerId = 1, AddressId = 1 },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf2", CustomerId = 1, AddressId = 2 }
            };

            foreach (var temp in CustomerDeliveryAddress)
            {
                context.CustomerDeliveryAddresses.Add(temp);
            }



            context.SaveChanges();
            var VATs = new List<VAT>
            {
                new VAT {VATValue= 0, VATPercId = 1 },
                new VAT {VATValue= 6, VATPercId = 2 },
                new VAT {VATValue= 12, VATPercId = 3 },
                new VAT {VATValue= 21, VATPercId= 4 }
            };
            foreach (var temp in VATs)
            {
                context.VATs.Add(temp);
            }
            context.SaveChanges();

            var Categories = new List<Category>
            {
                new Category { CategoryName="Dakpannen", CategoryId = 1 },
                new Category { CategoryName="Vijzen", CategoryId = 2 },
                new Category { CategoryName="Roofing", CategoryId = 3 },
                new Category { CategoryName="Hout", CategoryId = 4 }
            };
            foreach (var temp in Categories)
            {
                context.Categories.Add(temp);
            }
            context.SaveChanges();

            var Products = new List<Product>
            {
                new Product {ProductName="Dakpan", ProductCode="123456M", Remark="test", Description="Grijs", PriceExVAT= 148, Reprobel= 0, Bebat = 1, Auvibel = 22, Recupel = 32, PurchasePrice = 68, Brand = "Brico", CategoryId = 1, VATPercId = 3, Stock = 6, EAN= "123456789" },
                new Product {ProductName="Vijsje", ProductCode="987562T", Remark="test2", Description="Kruis", PriceExVAT= 6, Reprobel= 0, Bebat = 0, Auvibel = 0, Recupel = 0, PurchasePrice = 62, Brand = "Gamma", CategoryId = 2, VATPercId = 2, Stock = 105, EAN= "9876543210" }

            };
            foreach (var temp in Products)
            {
                context.Products.Add(temp);
            }
            context.SaveChanges();



            Company comp = new Company();
            comp.CompanyId = 1;
            comp.CompanyName = "De Brabander";
            comp.Street = "Ruggeveldlaan 12";
            comp.District = "Deurne";
            comp.Postalcode = "2100";
            comp.Email = "Joe@doe.com";
            comp.Country = "België";
            comp.VatNumber = "BE874598852";
            comp.Iban = "BE6585146633";
            comp.BIC = "KREDBEBB";
            comp.Phone = "03/325 55 88";
            comp.Mobile = "0497/ 22 55 99";
            comp.LogoLink = "@Dit is een link";
            context.Companies.Add(comp);


        }



    }
}