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


            


            Address add1 = new Address { StreetName = "Boekweitstraat", StreetNumber = 74,  PostalCodeNumber = 2900, Town = "Schoten"  };
            Address add2 = new Address { StreetName = "Bredabaan", StreetNumber = 813, PostalCodeNumber = 2170, Town = "Merksem"  };

         
            var Customers = new List<Customer>
            {
                new Customer { FirstName="Tom", LastName="Brunson", CompanyName="Brunson NV", Phone="05987415", Email="tom@brunsonnv.be", VATNumber="BE874598745", Address = add1, AccountNumber = "1", CellPhone="0471573796", CreationDate = DateTime.Now, Annotation="Mr.", TAXLiability= "true", Type="Aannemer", ContactName="Tom Brunson", ContactEmail="tom@brunsonnv.be", ContactCellPhone="0497182222"   },
                new Customer { FirstName="Maarten", LastName="DeVleesSchouwer", CompanyName="DV bvba", Phone="033225555", Email="maarten@dv.be", VATNumber="BE8745987444", Address = add1, AccountNumber="5", ContactCellPhone="0498556677", Annotation="Mr.", CreationDate = DateTime.Now, TAXLiability="false", Type="Particulier", ContactName="Maarten De Vleeschouwer", ContactEmail="maarten@dv.be", CellPhone="045597422" },
                new Customer { FirstName="Van De Walle", LastName="Michiel", Address = add1, CreationDate = DateTime.Now, CompanyName="Van de Walle NV", Phone="034598722", Email="michiel@vandewalle.be", VATNumber="BE89742251", ContactCellPhone="0497557799", Annotation="Mr.", Type="Aannemer", TAXLiability= "true", ContactName="Michiel Van De Walle", ContactEmail="michiel@vandewalle.be", AccountNumber="874211985", CellPhone="0486995514" },
                new Customer { FirstName="Glenn", LastName="Gersis", Address = add2, CreationDate = DateTime.Now, CompanyName="Gersis NV", Phone="88554477", Email="Glenn@gersisnv.com", VATNumber="BE158741225", ContactCellPhone="049725814", Annotation="Mr.", Type="Aannemer", TAXLiability= "true", ContactName="Glenn Gersis", ContactEmail="glenn@gersisnv.com", AccountNumber="8574", CellPhone="0497158844" }
            };

            foreach (var temp in Customers)
            {
                context.Customers.Add(temp);
            }
            context.SaveChanges();

            var AddressList= new List<Address>
            {
                new Address { StreetName = "Kerkhofweg", StreetNumber = 25, PostalCodeNumber = 2100, Town = "Deurne" },
                new Address { StreetName = "Boterlaarbaan", StreetNumber = 101, PostalCodeNumber = 2100, Town = "Deurne" },
                new Address { StreetName = "Kerkstraat", StreetNumber = 23, PostalCodeNumber = 2160, Town = "Wommelgem" },
                new Address { StreetName = "Shupstraat", StreetNumber = 25, PostalCodeNumber = 2018, Town = "Antwerpen" },
                new Address { StreetName = "Vosstraat", StreetNumber = 44, PostalCodeNumber = 2600, Town = "Berchem" },
                new Address { StreetName = "Boomsesteenweg", StreetNumber = 630, PostalCodeNumber = 2850, Town = "Boom" },
                new Address { StreetName = "Suikerdijkstraat", StreetNumber = 6, PostalCodeNumber = 2070, Town = "Zwijndrecht" },
                new Address { StreetName = "Rozenlaan", StreetNumber = 56, PostalCodeNumber = 2070, Town = "Burcht" },
            };
            foreach (var temp in AddressList)
            {
                context.Addresses.Add(temp);
            }

            context.SaveChanges();

            var CustomerDeliveryAddress = new List<CustomerDeliveryAddress>
            {
                new CustomerDeliveryAddress { DeliveryAddressInfo="Werf1", CustomerId = 1, StreetName = "Kerkhofweg", StreetNumber = 25, PostalCodeNumber = 2100, Town = "Deurne" },
                new CustomerDeliveryAddress { DeliveryAddressInfo="Werf4", CustomerId = 1, StreetName = "lekenstraat", StreetNumber = 30, PostalCodeNumber = 2600, Town = "boemerskonte" },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf2", CustomerId = 2, StreetName = "Boterlaarbaan", StreetNumber = 101, PostalCodeNumber = 2100, Town = "Deurne" },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf3", CustomerId = 3, StreetName = "Suikerdijkstraat", StreetNumber = 6, PostalCodeNumber = 2070, Town = "Zwijndrecht"  },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf5", CustomerId = 4, StreetName = "rederijstraat", StreetNumber = 77, PostalCodeNumber = 2550, Town = "Edegem"  }
            };

            foreach (var temp in CustomerDeliveryAddress)
            {
                context.CustomerDeliveryAddresses.Add(temp);
            }

            context.SaveChanges();

            var UserDefinedSettings = new List<UserDefinedSetting>
            {
                new UserDefinedSetting { IndexResultLength= 10, DetailsResultLength= 5, ExpirationDateLength=1 }
            };
            foreach (var temp in UserDefinedSettings)
            {
                context.UserDefinedSettings.Add(temp);
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
                new Category { CategoryName="Hout", CategoryId = 4 },
                new Category { CategoryName="Dakramen", CategoryId = 5 },
                new Category { CategoryName="Roofing", CategoryId=6 },
                new Category { CategoryName="Dakgoten", CategoryId=7 }
            };
            foreach (var temp in Categories)
            {
                context.Categories.Add(temp);
            }
            context.SaveChanges();

            var Products = new List<Product>
            {
                new Product {ProductName="Dakpan", ProductCode="123456M", Remark="test", Description="Grijs", PriceExVAT= 148, Reprobel= 0, Bebat = 1, Auvibel = 22, Recupel = 32, PurchasePrice = 68, Brand = "Brico", CategoryId = 1, VATPercId = 3, Stock = 6, EAN= "123456789" },
                new Product {ProductName="Vijsje", ProductCode="987562T", Remark="test2", Description="Kruis", PriceExVAT= 6, Reprobel= 0, Bebat = 0, Auvibel = 0, Recupel = 0, PurchasePrice = 62, Brand = "Gamma", CategoryId = 2, VATPercId = 2, Stock = 105, EAN= "9876543210" },
                new Product {ProductName="Dakraam", ProductCode="584G87", Remark="Grijs", Description="Dubbel glas", PriceExVAT= 6420, Reprobel= 8, Bebat = 0, Auvibel = 0, Recupel = 0, PurchasePrice = 99, Brand = "Belisol", CategoryId = 5, VATPercId = 4, Stock = 3, EAN= "5871254" },
                new Product {ProductName="Balken (Hout)", ProductCode="HOUT885", Remark="Den", Description="Ongeschaafd", PriceExVAT= 62, Reprobel= 0, Bebat = 0, Auvibel = 0, Recupel = 0, PurchasePrice = 15, Brand = "Crollet", CategoryId = 4, VATPercId = 4, Stock = 201, EAN= "EAN778415" },
                new Product {ProductName="Roofing", ProductCode="R88745", Remark="Zwart", Description="Roofing", PriceExVAT= 95, Reprobel= 0, Bebat = 15, Auvibel = 0, Recupel = 0, PurchasePrice = 33, Brand = "Sencys", CategoryId = 6, VATPercId = 3, Stock = 56, EAN= "58745221" },
                new Product {ProductName="Dakgoot", ProductCode="D874154", Remark="Inox", Description="Lang", PriceExVAT= 79, Reprobel= 0, Bebat = 0, Auvibel = 0, Recupel = 0, PurchasePrice = 21, Brand = "Martens", CategoryId = 7, VATPercId = 4, Stock = 12, EAN= "4412570" }

            };
            foreach (var temp in Products)
            {
                context.Products.Add(temp);
            }
            context.SaveChanges();



            Company comp = new Company();
            comp.CompanyId = 1;
            comp.CompanyName = "De Brabander bvba";
            comp.Street = "Lispersteenweg 91";
            comp.District = "Boechout";
            comp.Postalcode = "2530";
            comp.Email = " info@debrabanderdakwerken.be";
            comp.Country = "België";
            comp.VatNumber = "BE874598852";
            comp.Iban = "BE6585146633";
            comp.BIC = "KREDBEBB";
            comp.Phone = "03 288 00 99";
            comp.Mobile = "0477 318 372";
            
            context.Companies.Add(comp);


        }

        

    }
}