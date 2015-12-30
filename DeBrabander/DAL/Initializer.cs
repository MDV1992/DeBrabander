using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DeBrabander.Models;
using System.Data.Entity.Infrastructure;

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

            var PostalCodes = new List<PostalCode>
            {
                new PostalCode {PostalCodeNumber = 2900, Town="Schoten" },
                new PostalCode {PostalCodeNumber= 2170, Town="Merksem" },
                new PostalCode {PostalCodeNumber = 2930 , Town="Brasschaat" }
            };

            foreach (var temp in PostalCodes)
            {
                context.PostalCodes.Add(temp);
            }

            context.SaveChanges();

            var Addresses = new List<Address>
            {
                new Address { StreetName ="Boekweitstraat", StreetNumber=74 , PostalCodeId=1  },
                new Address { StreetName="Bredabaan", StreetNumber= 813, PostalCodeId=2 }
            };

            foreach (var temp in Addresses)
            {
                context.Addresses.Add(temp);
            }

            context.SaveChanges();

            var Customers = new List<Customer>
            {
                new Customer { FirstName="Tom", LastName="Brunson", AddressId = 1 },
                new Customer { FirstName="Maarten", LastName="DeVleesSchouwer", AddressId = 2 },
                new Customer { FirstName="Glenn", LastName="Gersis", AddressId = 1 }
            };

            foreach (var temp in Customers)
            {
                context.Customers.Add(temp);
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
        }
    }
}