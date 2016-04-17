using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        [DisplayName("Factuur nummer")]
        public int InvoiceNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [DisplayName("Totaal prijs")]
        public double TotalPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Aangemaakt op")]
        public DateTime Date { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Opmerking")]
        public string Annotation { get; set; }
        [DisplayName("Actief")]
        public bool Active { get; set; }
        public int CustomerId { get; set; }
        // [DisplayName] zorgt ervoor dat het aangegeven argument 
        // wordt weergegeven in de view ipv property naam
        [DisplayName("Naam")]
        public string LastName { get; set; }
        [DisplayName("Voornaam")]
        public string FirstName { get; set; }
        [NotMapped]
        [DisplayName("Naam")]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
        [DisplayName("GSM")]
        public string CellPhone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Straatnaam")]
        public string StreetName { get; set; }
        [DisplayName("Huisnummer")]
        public int StreetNumber { get; set; }
        [DisplayName("Busnummer")]
        public int Box { get; set; }
        [DisplayName("Postcode")]
        public int PostalCodeNumber { get; set; }
        [DisplayName("Gemeente")]
        public string Town { get; set; }
        public virtual CustomerDeliveryAddress customerDeliveryAddress { get; set; }
        public virtual List<InvoiceDetail> InvoiceDetail { get; set; }
    }
}