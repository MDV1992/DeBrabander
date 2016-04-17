using DeBrabander.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeBrabander.ViewModels.Customers
{
    public class CustomerEditViewModel
    {
        public Customer customer = new Customer();
    }
}