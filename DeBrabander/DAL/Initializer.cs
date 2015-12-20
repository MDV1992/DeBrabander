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
            base.Seed(context);
        }
    }
}