using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DeBrabander.Models;

namespace DeBrabander.ViewModels.Customers
{
    public class CustomerCreateViewModel
    {
        public Customer customer = new Customer();

    }
}