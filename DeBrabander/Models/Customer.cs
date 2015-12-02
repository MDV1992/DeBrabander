using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        public string naam { get; set; }
        public string voornaam { get; set; }

    }
}