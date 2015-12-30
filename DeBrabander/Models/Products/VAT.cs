using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class VAT
    {
        [Key]
        public int VATPercId { get; set; }

        [DisplayName("BTW Percentage")]
        public double VATValue { get; set; }
    }
}