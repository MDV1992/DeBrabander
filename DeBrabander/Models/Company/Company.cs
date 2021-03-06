﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DeBrabander.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        [DisplayName("Bedrijfsnaam")]
        public string CompanyName { get; set; }
        [DisplayName("Straat")]
        public string Street { get; set; }
        [DisplayName("Postcode")]
        public string Postalcode { get; set; }
        [DisplayName("Gemeente")]
        public string District { get; set; }
        [DisplayName("Telefoon")]
        public string Phone { get; set; }
        [DisplayName("GSM")]
        public string Mobile { get; set; }
        [DisplayName("E-mail")]
        public string Email {get; set;}
        [DisplayName("Land")]
        public string Country { get; set; }
        [DisplayName("BTW-nummer")]
        public string VatNumber { get; set; }
        [DisplayName("Bank-rekening")]
        public string Iban { get; set; }
        [DisplayName("BIC")]
        public string BIC { get; set; }
        [DisplayName("Website")]
        public string Website { get; set; }
      
        public Company company { get; set; }

        }


    }
