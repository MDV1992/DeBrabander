using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;

namespace DeBrabander.ViewModels.QuotationDetails
{
    public class QuotationDetailsCreateViewModel
    {
        public QuotationDetail QuotationDetail { get; set; }
        public Quotation Quotation { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}