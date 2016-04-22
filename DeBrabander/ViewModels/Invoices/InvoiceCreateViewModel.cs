using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;

namespace DeBrabander.ViewModels.Invoices
{
    public class InvoiceCreateViewModel
    {
        public Invoice invoice = new Invoice();
       
        public Customer customer { get; set; }
    }
}