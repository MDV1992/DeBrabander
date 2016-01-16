using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class QuotationDetail
    {
        [Key]
        public int QuotationDetailId { get; set; }
        [DisplayName("Productcode")]
        public string ProductCode { get; set; }
        [DisplayName("Product Naam")]
        public string ProductName { get; set; }
        public int UnitPrice { get; set; }
        [DisplayName("Aantal")]
        public int Quantity { get; set; }
    }
}