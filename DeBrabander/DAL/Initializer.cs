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
            Address add3 = new Address { StreetName = "Hoveniersstraat", StreetNumber = 15, PostalCodeNumber = 2018, Town = "Antwerpen" };
            Address add4 = new Address { StreetName = "Plantin en Moretuslei", StreetNumber = 23, PostalCodeNumber = 2018, Town = "Antwerpen" };
            Address add5 = new Address { StreetName = "Krijgsbaan", StreetNumber = 328, PostalCodeNumber = 2100, Town = "Deurne" };
            Address add6 = new Address { StreetName = "Medialaan", StreetNumber = 26, PostalCodeNumber = 1800, Town = "Vilvoorde" };
            Address add7 = new Address { StreetName = "Mechelsesteenweg", StreetNumber = 118, PostalCodeNumber = 2800, Town = "Mechelen" };
            Address add8 = new Address { StreetName = "Boterlaarbaan", StreetNumber = 2, PostalCodeNumber = 2100, Town = "Deurne" };
            Address add9 = new Address { StreetName = "Beursplein", StreetNumber = 44, PostalCodeNumber = 1000, Town = "Brussel" };
            Address add10 = new Address { StreetName = "Nieuwmoersesteenweg", StreetNumber = 95, PostalCodeNumber = 2990, Town = "Wuustwezel" };
            Address add11 = new Address { StreetName = "Stationsstraat", StreetNumber = 9, PostalCodeNumber = 2910, Town = "Essen" };
            Address add12 = new Address { StreetName = "Tulpenlaan", StreetNumber = 62, PostalCodeNumber = 9150, Town = "Kruibeke" };
            Address add13 = new Address { StreetName = "Bisschoppenhoflaan", StreetNumber =284, PostalCodeNumber = 2100, Town = "Deurne" };


            var Customers = new List<Customer>
            {
                new Customer { FirstName="Tom", LastName="Brunson", CompanyName="Brunson NV", Phone="05987415", Email="tom@brunsonnv.be", VATNumber="BE874598745", Address = add1, AccountNumber = "1", CellPhone="0471573796", CreationDate = DateTime.Now, Annotation="Mr.", TAXLiability= "true", Type="Aannemer", ContactName="Tom Brunson", ContactEmail="tom@brunsonnv.be", ContactCellPhone="0497182222"  },
                new Customer { FirstName="Maarten", LastName="DeVleesSchouwer", CompanyName="DV bvba", Phone="033225555", Email="maarten@dv.be", VATNumber="BE8745987444", Address = add4, AccountNumber="5", ContactCellPhone="0498556677", Annotation="Mr.", CreationDate = DateTime.Now, TAXLiability="false", Type="Particulier", ContactName="Maarten De Vleeschouwer", ContactEmail="maarten@dv.be", CellPhone="045597422" },
                new Customer { FirstName="Van De Walle", LastName="Michiel", Address = add7, CreationDate = DateTime.Now, CompanyName="Van de Walle NV", Phone="034598722", Email="michiel@vandewalle.be", VATNumber="BE89742251", ContactCellPhone="0497557799", Annotation="Mr.", Type="Aannemer", TAXLiability= "true", ContactName="Michiel Van De Walle", ContactEmail="michiel@vandewalle.be", AccountNumber="874211985", CellPhone="0486995514" },
                new Customer { FirstName="Glenn", LastName="Gersis", Address = add5, CreationDate = DateTime.Now, CompanyName="Gersis NV", Phone="88554477", Email="Glenn@gersisnv.com", VATNumber="BE158741225", ContactCellPhone="049725814", Annotation="Mr.", Type="Aannemer", TAXLiability= "true", ContactName="Glenn Gersis", ContactEmail="glenn@gersisnv.com", AccountNumber="8574", CellPhone="0497158844" },
                new Customer { FirstName="Sophie", LastName="Van de Perre", Address = add2, CreationDate = DateTime.Now, CompanyName="Perre BVBA", Phone="8874123545", Email="sofie.perre@perrebvba.be", VATNumber="BE871244", ContactCellPhone="048622581", Annotation="Mvr.", Type="Particulier", TAXLiability= "true", ContactName="Sofie Van De Perre", ContactEmail="sofie.vandeperre@Perrebvba.be", AccountNumber="87125471", CellPhone="048742251" },
                new Customer { FirstName="Rudy", LastName="Vervoort", Address = add6, CreationDate = DateTime.Now, CompanyName="Vervoort", Phone="82242455", Email="rudy.vervoort@gmail.com", VATNumber="BE1824110", ContactCellPhone="048633541", Annotation="Mr.", Type="Aannemer", TAXLiability= "False", ContactName="Hans Anders", ContactEmail="hans.anders@gmail.com", AccountNumber="77419", CellPhone="049633271" },
                new Customer { FirstName="Jan", LastName="Op de Beeck", Address = add3, CreationDate = DateTime.Now, CompanyName="Op de Beeck bvba", Phone="021544712", Email="info@opdebeeck.be", VATNumber="BE685142", ContactCellPhone="0486335210", Annotation="Mr.", Type="Aannemer", TAXLiability= "true", ContactName="jan.opdebeeck@gmail.com", ContactEmail="jan.opdebeeck@gmail.com", AccountNumber="996521", CellPhone="086352241" },
                new Customer { FirstName="Danny", LastName="Peeters", Address = add8, CreationDate = DateTime.Now, CompanyName="Peeters NV", Phone="7441254", Email="danny@peetersnv.nl", VATNumber="BE5245241", ContactCellPhone="04869978411", Annotation="Mr.", Type="Aannemer", TAXLiability= "true", ContactName="Danny Peeters", ContactEmail="danny@peetersnv.nl", AccountNumber="8521445", CellPhone="048633741" },
                new Customer { FirstName="Gitte", LastName="van den Bosche", Address = add9, CreationDate = DateTime.Now, CompanyName="Bosche bvba", Phone="12357410", Email="Gitte@vandenbosche.be", VATNumber="BE4125841", ContactCellPhone="0497855236", Annotation="Mvr.", Type="Aannemer", TAXLiability= "true", ContactName="Gitte van den Bosche", ContactEmail="gitte@vandenbosche.nl", AccountNumber="88422", CellPhone="049755310" },
                new Customer { FirstName="Julie", LastName="van Aalst", Address = add10, CreationDate = DateTime.Now, CompanyName="Van Aalst nv", Phone="0154025431", Email="julie.vanaalst@telenet.be", VATNumber="BE6652014", ContactCellPhone="0486221011", Annotation="Mr.", Type="Aannemer", TAXLiability= "true", ContactName="Julie van Aalst", ContactEmail="julie.vanaalst@telenet.be", AccountNumber="774165", CellPhone="048622014" },
                new Customer { FirstName="Harry", LastName="Kerkstoel", Address = add11, CreationDate = DateTime.Now, CompanyName="Kerkstoel NV", Phone="774155", Email="harry@kerkstoel.be", VATNumber="BE4774156361", ContactCellPhone="04984542", Annotation="Mr.", Type="Particulier", TAXLiability= "true", ContactName="Harry Kerkstoel", ContactEmail="harry@kerkstoel.be", AccountNumber="112458", CellPhone="08622105" },
                new Customer { FirstName="Eline", LastName="Janssens", Address = add12, CreationDate = DateTime.Now, CompanyName="Jannsens Cosmetics", Phone="9822122", Email="Eline@janssens.be", VATNumber="BE8742369", ContactCellPhone="0486995201", Annotation="Mvr.", Type="Aannemer", TAXLiability= "true", ContactName="Eline Janssens", ContactEmail="eline@janssens.be", AccountNumber="45514211", CellPhone="048633521" },
                new Customer { FirstName="Sara", LastName="Wortel", Address = add13, CreationDate = DateTime.Now, CompanyName="Wortel bvba", Phone="55125245", Email="Sara.wortel@proximus.be", VATNumber="BE4885214", ContactCellPhone="047254189", Annotation="Mvr.", Type="Aannemer", TAXLiability= "false", ContactName="Sara Wortel", ContactEmail="sara.wortel@proximus.be", AccountNumber="320121244", CellPhone="07252456482" }








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
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf5", CustomerId = 4, StreetName = "rederijstraat", StreetNumber = 77, PostalCodeNumber = 2550, Town = "Edegem"  },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf6", CustomerId = 4, StreetName = "Teststraat", StreetNumber = 88, PostalCodeNumber = 1742, Town = "Ternat"  },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf7", CustomerId = 5, StreetName = "Krijgsbaan", StreetNumber = 26, PostalCodeNumber = 1800, Town = "Vilvoorde"  },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf8", CustomerId = 6, StreetName = "Mechelsesteenweg", StreetNumber = 188, PostalCodeNumber = 2800, Town = "Mechelen"  },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf9", CustomerId = 7, StreetName = "Boterlaarbaan", StreetNumber = 2, PostalCodeNumber = 2100, Town = "Deurne"  },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf10", CustomerId = 8, StreetName = "Beursplein", StreetNumber = 44, PostalCodeNumber = 1000, Town = "Brussel"  },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf11", CustomerId = 9, StreetName = "Lodewijkstraat", StreetNumber = 107, PostalCodeNumber = 2060, Town = "Wommelgem"  },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf12", CustomerId = 10, StreetName = "Nieuwmoersesteenweg", StreetNumber = 95, PostalCodeNumber = 2990, Town = "Wuustwezel"  },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf 13", CustomerId= 11, StreetName = "Stationsstraat", StreetNumber = 9, PostalCodeNumber = 2910, Town = "Essen" },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf 14", CustomerId = 12,  StreetName = "Tulpenlaan", StreetNumber = 62, PostalCodeNumber = 9150, Town = "Kruibeke"},
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf 15", CustomerId = 13, StreetName = "Bisschoppenhoflaan", StreetNumber =284, PostalCodeNumber = 2100, Town = "Deurne" },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf16", CustomerId= 9, StreetName = "Buizerdlaan", StreetNumber = 9, PostalCodeNumber = 2100, Town="Deurne" },
                new CustomerDeliveryAddress { DeliveryAddressInfo="werf17", CustomerId= 10, StreetName = "Distelvinklaan", StreetNumber = 34, PostalCodeNumber = 2660, Town="Hoboken" }

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
                new Product {ProductName="Dakpan", ProductCode="123456M", Remark="test", Description="Grijs", PriceExVAT= 148, Reprobel= 0, Bebat = 1, Auvibel = 22, Recupel = 32, PurchasePrice = 68, Brand = "Brico", CategoryId = 1, VATPercId = 3, Stock = 6, EAN= "123456789", Active = true },
                new Product {ProductName="Vijsje", ProductCode="987562T", Remark="test2", Description="Kruis", PriceExVAT= 6, Reprobel= 0, Bebat = 0, Auvibel = 0, Recupel = 0, PurchasePrice = 62, Brand = "Gamma", CategoryId = 2, VATPercId = 2, Stock = 105, EAN= "9876543210", Active = true },
                new Product {ProductName="Dakraam", ProductCode="584G87", Remark="Grijs", Description="Dubbel glas", PriceExVAT= 6420, Reprobel= 8, Bebat = 0, Auvibel = 0, Recupel = 0, PurchasePrice = 99, Brand = "Belisol", CategoryId = 5, VATPercId = 4, Stock = 3, EAN= "5871254", Active = true },
                new Product {ProductName="Balken (Hout)", ProductCode="HOUT885", Remark="Den", Description="Ongeschaafd", PriceExVAT= 62, Reprobel= 0, Bebat = 0, Auvibel = 0, Recupel = 0, PurchasePrice = 15, Brand = "Crollet", CategoryId = 4, VATPercId = 4, Stock = 201, EAN= "EAN778415", Active = true },
                new Product {ProductName="Roofing", ProductCode="R88745", Remark="Zwart", Description="Roofing", PriceExVAT= 95, Reprobel= 0, Bebat = 15, Auvibel = 0, Recupel = 0, PurchasePrice = 33, Brand = "Sencys", CategoryId = 6, VATPercId = 3, Stock = 56, EAN= "58745221", Active = true },
                new Product {ProductName="Dakgoot", ProductCode="D874154", Remark="Inox", Description="Lang", PriceExVAT= 79, Reprobel= 0, Bebat = 0, Auvibel = 0, Recupel = 0, PurchasePrice = 21, Brand = "Martens", CategoryId = 7, VATPercId = 4, Stock = 12, EAN= "4412570", Active = true }

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