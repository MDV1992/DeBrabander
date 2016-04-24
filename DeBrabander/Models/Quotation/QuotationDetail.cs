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
        [DisplayName("Aantal")]
        public int Quantity { get; set; }
        public int QuotationId { get; set; }
        public int ProductId { get; set; }
        [DisplayName("Product Naam")]
        public string ProductName { get; set; }
        [DisplayName("Productcode")]
        public string ProductCode { get; set; }
        [DisplayName("Omschrijving")]
        public string Description { get; set; }
        [DisplayName("Prijs exclusief BTW")]
        public double PriceExVAT { get; set; }
        [DisplayName("Reprobel")]
        public double Reprobel { get; set; }
        [DisplayName("BebatT")]
        public double Bebat { get; set; }
        [DisplayName("Recupel")]
        public double Recupel { get; set; }
        [DisplayName("Auvibel")]
        public double Auvibel { get; set; }
        [DisplayName("Merk")]
        public string Brand { get; set; }
        [DisplayName("Categorie")]
        public int CategoryId { get; set; }
        [DisplayName("BTW %")]
        public int VATPercId { get; set; }
        [DisplayName("Totaal Exclusief BTW")]
        public double TotalExVat { get; set; }
        [DisplayName("Totaal Inclusief BTW")]
        public double TotalIncVat { get; set; }
        
        public virtual VAT VAT { get; set; }
    }
}