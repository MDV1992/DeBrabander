using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;

namespace DeBrabander.ViewModels.Orders
{
    public class OrderEditViewModel
    {
        public Quotation quotation = new Quotation();
        public Order order = new Order();
        public Customer customer = new Customer();        
        public OrderDetail orderDetail = new OrderDetail();
        public Product product = new Product();
    }
}