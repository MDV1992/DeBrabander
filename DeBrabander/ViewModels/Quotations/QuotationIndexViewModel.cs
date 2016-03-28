using DeBrabander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace DeBrabander.ViewModels.Quotations
{
    public class QuotationIndexViewModel
    {
        public Quotation quotation = new Quotation();
        public IPagedList<Quotation> quotations { get; set; }
        public Customer customer = new Customer();
    }
}