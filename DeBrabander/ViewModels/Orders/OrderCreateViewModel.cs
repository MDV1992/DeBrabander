using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;
using PagedList;

namespace DeBrabander.ViewModels
{
    public class OrderCreateViewModel
    {
        public Order order = new Order();
        public IPagedList<Customer> customers { get; set; }
        public Customer customer { get; set; }
    }
}