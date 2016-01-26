using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeBrabander.ViewModels.Products
{
    public class ProductCreateViewModel
    {
        public int ProductId { get; set; }
        [DisplayName ("Productnaam")]
        public string ProductName { get; set; }
        [DisplayName("Productcode")]
        public string ProductCode { get; set; }
        [DisplayName("Opmerking")]
        public string Remark { get; set; }
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
        [DisplayName("Aankoopprijs")]
        public double PurchasePrice { get; set; }
        [DisplayName("Merk")]
        public string Brand { get; set; }
        [DisplayName("Categorie")]
        public int CategoryId { get; set; }
       
        [DisplayName("BTW %")]
        public int VATPercId { get; set; }
        [DisplayName("Voorraad")]
        public double Stock { get; set; }
        [DisplayName("EAN Code")]
        public string EAN { get; set; }
        [DisplayName("Voorraad Controle")]
        public bool StockControl { get; set; }
        [DisplayName("Categorie")]
        public string CategoryName { get; set; }
        [DisplayName("BTW Percentage")]
        public double VATValue { get; set; }
        
        
    }
}