using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;
using PagedList;

namespace DeBrabander.ViewModels
{
    public class OrderAddProductsViewModel
    {
        public Order order = new Order();
        public OrderDetail orderDetail = new OrderDetail();

        // voor lijst producten in add products +
        public IPagedList<Product> products { get; set; }
        public Product product = new Product();
    }
}