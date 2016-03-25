using DeBrabander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeBrabander.ViewModels.Quotations
{
    public class QuotationIndexViewModel
    {
        public Quotation quotation = new Quotation();
        public Customer customer = new Customer();
    }
}