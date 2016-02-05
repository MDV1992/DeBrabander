using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DeBrabander.Models.Company
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string District { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email {get; set;}
        public string Country { get; set; }
        public string VatNumber { get; set; }
        public string Iban { get; set; }
        public string BIC { get; set; }
        public byte LogoLink { get; set; }


    }
}