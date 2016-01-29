using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [DisplayName("WerfAdresID")]
        public int AddressId { get; set; }
    }
}