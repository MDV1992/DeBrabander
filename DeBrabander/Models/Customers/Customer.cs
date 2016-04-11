﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DeBrabander.Models
{
    public class Customer
    {
        [Key]
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
        public DateTime CreationDate { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("BTW Plichtig")]
        public string TAXLiability { get; set; }
        
        public virtual Address Address { get; set; }
        
        public virtual List<CustomerDeliveryAddress> CustomerDeliveryAddress { get; set; }

        
    }
}