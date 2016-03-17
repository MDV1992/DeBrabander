using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;

namespace DeBrabander.ViewModels.Quotations
{
    public class QuotationEditViewModel
    {
        public Quotation quotation = new Quotation();
        public Product product = new Product();
        public QuotationDetail quotationDetail = new QuotationDetail();
    }
}