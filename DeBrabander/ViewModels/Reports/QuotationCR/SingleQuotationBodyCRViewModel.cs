using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeBrabander.ViewModels
{
    public class SingleQuotationBodyCRViewModel
    {
        public int QuotationDetailId { get; set; }
        public int Quantity { get; set; }
        public int QuotationId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public double PriceExVAT { get; set; }
        public double Reprobel { get; set; }
        public double Bebat { get; set; }
        public double Recupel { get; set; }
        public double Auvibel { get; set; }
        public string Brand { get; set; }
        public string CategoryName { get; set; }
        public int VATValue { get; set; }
        
    }
}