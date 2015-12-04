using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using DeBrabander.Models;

namespace DeBrabander.DAL
{
    

    public class Context : DbContext
    {
        public Context() : base("DefaultConnection")
        {
        }

        public DbSet<UserAccount> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //Tabelnamen worden nu niet meer in meervouden gezet.
        }




    }
}