using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;
using PagedList;

namespace DeBrabander.ViewModels
{
    public class InvoiceIndexViewModel
    {
        public Invoice invoice = new Invoice();
        public IPagedList<Invoice> invoices { get; set; }
        public Customer customer = new Customer();
    }
}