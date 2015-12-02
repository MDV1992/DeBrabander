using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DeBrabander.Models;

namespace DeBrabander.DAL
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            var user = new List<User>
            {
                new User { Login = "Tom", Password = "123", rights = 1 }
            };
        }
    }
}