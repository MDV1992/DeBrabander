﻿using System;
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
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class Context : DbContext
    {
        public Context() : base("DefaultConnection")
        {
            Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());
        }

        static Context()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Quotation> Quotations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Tabelnamen worden nu niet meer in meervouden gezet.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}