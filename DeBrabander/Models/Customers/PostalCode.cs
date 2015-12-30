using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class PostalCode
    {
        [Key]
        [DisplayName("Postcode Id")]
        public int PostalCodeId { get; set; }

        [DisplayName("Postcode")]
        public int PostalCodeNumber { get; set; }

        [DisplayName("Gemeente")]
        public string Town { get; set; }
    }
}