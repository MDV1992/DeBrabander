using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using DeBrabander.Models;

using MySql.Data.MySqlClient;
using MySql.Data.Entity;


namespace DeBrabander.DAL
{
    // Indien het scaffolding niet lukt http://stackoverflow.com/questions/12546545/unable-to-retrieve-metadata
    // verander in web.config   "MySql.Data.MySqlClient" in "System.Data.SqlClient"  en nadien terug zetten
    //Static context uit comments halen, DBconfiguration terug in comment zetten.

  [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class Context : DbContext
    {
        public Context() : base("DefaultConnection")
        {
            Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());
        }

        static Context()
        {
          //  Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set;}
        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<VAT> VATs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<QuotationDetail> QuotationDetails { get; set; }

        public DbSet<CustomerDeliveryAddress> CustomerDeliveryAddresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Tabelnamen worden nu niet meer in meervouden gezet.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public System.Data.Entity.DbSet<DeBrabander.Models.Company.Company> Companies { get; set; }
    }
}