using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class Address
    {
        [Key]
        [DisplayName("Adres ID")]
        public int AddressId { get; set; }

        [DisplayName("Straatnaam")]
        public string StreetName { get; set; }

        [DisplayName("Huisnummer")]
        public int StreetNumber { get; set; }

        [DisplayName("Busnummer")]
        public int Box { get; set; }

        [DisplayName("PostCodeId")]
        public int PostalCodeId { get; set; }

        public PostalCode PostalCode { get; set; }
  
    }
}