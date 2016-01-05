using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeBrabander.ViewModels.Customers
{
    public class CustomerDetailsViewModel
    {
        public int CustomerId { get; set; }
        // [DisplayName] zorgt ervoor dat het aangegeven argument 
        // wordt weergegeven in de view ipv property naam
        [DisplayName("Naam")]
        public string LastName { get; set; }
        [DisplayName("Voornaam")]
        public string FirstName { get; set; }
        [NotMapped]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }

        [DisplayName("Bedrijfsnaam")]
        public string CompanyName { get; set; }

        [DisplayName("Telefoon")]
        public string Phone { get; set; }

        [DisplayName("GSM")]
        public string CellPhone { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("BTW Nummer")]
        public string VATNumber { get; set; }

        [DisplayName("Rekeningnummer")]
        public string AccountNumber { get; set; }

        [DisplayName("Opmerking")]
        public string Annotation { get; set; }

        [DisplayName("Contact Persoon")]
        public string ContactName { get; set; }

        [DisplayName("Contact Email")]
        public string ContactEmail { get; set; }

        [DisplayName("Contact GSM")]
        public string ContactCellPhone { get; set; }

        [DisplayName("Creatie Datum")]
        public string CreationDate { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("BTW Plichtig")]
        public string TAXLiability { get; set; }

        [DisplayName("Adres ID")]
        public int AddressId { get; set; }

        [DisplayName("Straatnaam")]
        public string StreetName { get; set; }

        [DisplayName("Huisnummer")]
        public int StreetNumber { get; set; }

        [DisplayName("Busnummer")]
        public int Box { get; set; }

        [DisplayName("Postcode Id")]
        public int PostalCodeId { get; set; }

        [DisplayName("Postcode")]
        public int PostalCodeNumber { get; set; }

        [DisplayName("Gemeente")]
        public string Town { get; set; }
    }
}