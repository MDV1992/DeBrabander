using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeBrabander.ViewModels
{
    public class CustomerDeliveryAdressesCRViewModel
    {
        public int CustomerDeliveryAddressId { get; set; }
        public string DeliveryAddressInfo { get; set; }
        public int CustomerId { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public int Box { get; set; }
        public int PostalCodeNumber { get; set; }
        public string Town { get; set; }

    }
}