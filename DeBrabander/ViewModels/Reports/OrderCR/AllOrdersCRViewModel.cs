using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeBrabander.ViewModels
{
    public class AllOrdersCRViewModel
    {
        public int OrderId { get; set; }
        
        public int OrderNumber { get; set; }
       
        public double TotalPrice { get; set; }
      
        public DateTime Date { get; set; }
       
        public string Annotation { get; set; }

        public bool Active { get; set; }
        public int CustomerId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string CellPhone { get; set; }
 
        public string Email { get; set; }
 
        public string StreetName { get; set; }
   
        public int StreetNumber { get; set; }
 
        public int Box { get; set; }

        public int PostalCodeNumber { get; set; }

        public string Town { get; set; }


    }
}