using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;

namespace DeBrabander.ViewModels.Invoices
{
    public class InvoiceEditViewModel
    {
        public Invoice invoice = new Invoice();
        public Customer customer = new Customer();
        public Address address = new Address();
        public InvoiceDetail invoiceDetail = new InvoiceDetail();
        public Product product = new Product();
    }
}