using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeBrabander.ViewModels
{
    public class ProductCRViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Remark { get; set; }
        public string Description { get; set; }
        public double PriceExVAT { get; set; }
        public double Reprobel { get; set; }
        public double Bebat { get; set; }
        public double Recupel { get; set; }
        public double Auvibel { get; set; }
        public double PurchasePrice { get; set; }
        public string Brand { get; set; }
        public int CategoryId { get; set; }
        public int VATPercId { get; set; }
        public double Stock { get; set; }
        public string EAN { get; set; }

        public bool Active { get; set; }

        public string CategoryName { get; set; }
        public double VATValue { get; set; }




    }
}