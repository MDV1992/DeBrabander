using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DeBrabander.Models;
using PagedList;

namespace DeBrabander.ViewModels.Customers
{
    public class CustomerIndexViewModel
    {

        public IPagedList<Customer> customers { get; set; }
        
        public Customer customer = new Customer();
    }
}