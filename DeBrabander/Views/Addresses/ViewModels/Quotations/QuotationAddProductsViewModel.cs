using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;
using PagedList;

namespace DeBrabander.ViewModels.Quotations
{
    public class QuotationAddProductsViewModel
    {
        public Quotation quotation = new Quotation();
        public QuotationDetail quotationDetail = new QuotationDetail();

        // voor lijst producten in add products +
        public IPagedList<Product> products { get; set; }
        public Product product = new Product();
    }
}