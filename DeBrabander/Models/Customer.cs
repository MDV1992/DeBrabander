using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        // [DisplayName] zorgt ervoor dat het aangegeven argument 
        // wordt weergegeven in de view ipv property naam
        [DisplayName("Naam")]
        public string LastName { get; set; }
        [DisplayName("Voornaam")]
        public string FirstName { get; set; }        
    }
}