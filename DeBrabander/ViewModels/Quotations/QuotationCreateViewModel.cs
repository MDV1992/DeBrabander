using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;
using PagedList;

namespace DeBrabander.ViewModels.Quotations
{
    public class QuotationCreateViewModel
    {
        public Quotation quotation = new Quotation();
        public IPagedList<Customer> customers { get; set; }
        public Customer customer { get; set; }
    }
}