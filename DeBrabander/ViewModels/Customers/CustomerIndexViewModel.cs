using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DeBrabander.Models;

namespace DeBrabander.ViewModels.Customers
{
    public class CustomerIndexViewModel
    {

        public Customer customer = new Customer();
        public Address address = new Address();
        public PostalCode postalcode = new PostalCode();
    }
}