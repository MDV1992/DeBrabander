using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;

namespace DeBrabander.ViewModels.Customers
{
    public class CustomerAddDeliveryAddressViewModel
    {
        public Customer customer = new Customer();
        public CustomerDeliveryAddress deliveryAddress = new CustomerDeliveryAddress();
        public Address address = new Address();

    }
}