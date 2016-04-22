using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DeBrabander.Models;
using System.ComponentModel.DataAnnotations;

namespace DeBrabander.ViewModels.Customers
{
    public class CustomerDetailsViewModel 
    {

        public Customer customer = new Customer();
        
    }
}