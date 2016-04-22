using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;
using PagedList;

namespace DeBrabander.ViewModels.Orders
{
    public class OrderIndexViewModel
    {
        public Order order = new Order();
        public IPagedList<Order> orders { get; set; }
        public Customer customer = new Customer();
    }
}