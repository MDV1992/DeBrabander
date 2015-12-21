using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class Quotation
    {
        public int QuotationID { get; set; }
        [DisplayName("Offerte nummer")]
        public int QuotationNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DisplayName("Totaal prijs")]
        public int TotalPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Aangemaakt op")]
        public DateTime Date { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Vervalt op")]
        public DateTime ExpirationDate { get; set; }
        [DisplayName("Opmerking")]
        public string Annotation { get; set; }
        [DisplayName("Actief")]
        public bool Active { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
    }
}