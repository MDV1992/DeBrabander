using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class CustomerDeliveryAddress
    {
        [Key]
        [DisplayName("Werf Id")]
        public int CustomerDeliveryAddressId { get; set; }

        [DisplayName("Werf Info")]
        public string DeliveryAddressInfo { get; set; }

        [DisplayName("Klantnummer")]
        public int CustomerId { get; set; }

        [DisplayName("Straatnaam")]
        public string StreetName { get; set; }

        [DisplayName("Huisnummer")]
        public int StreetNumber { get; set; }

        [DisplayName("Busnummer")]
        public int Box { get; set; }

        [DisplayName("Postcode")]
        public int PostalCodeNumber { get; set; }

        [DisplayName("Gemeente")]
        public string Town { get; set; }




    }
}