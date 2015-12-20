using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class Quotation
    {
        public int QuotationID { get; set; }
        public int QuotationNumber { get; set; }
        public int TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Annotation { get; set; }
        public bool Active { get; set; }
        public List<Customer> Customers {get; set;}

    }
}