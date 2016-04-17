using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeBrabander.ViewModels
{
    public class AllCustomersCRViewModel
    {
        public int CustomerId { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public string VATNumber { get; set; }

        public string AccountNumber { get; set; }

        public string Annotation { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactCellPhone { get; set; }

        public DateTime CreationDate { get; set; }

        public string Type { get; set; }

        public string TAXLiability { get; set; }
        public int AddressId { get; set; }

        public string StreetName { get; set; }
        public int StreetNumber { get; set; }

        public int Box { get; set; }

        public int PostalCodeNumber { get; set; }

        public string Town { get; set; }

    }
}