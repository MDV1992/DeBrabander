using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeBrabander.ViewModels.Invoices
{
    public class SingleInvoiceCRViewModel
    {
        //company info
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string Postalcode { get; set; }
        public string District { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string EmailCompany { get; set; }
        public string Country { get; set; }
        public string VatNumber { get; set; }
        public string Iban { get; set; }
        public string BIC { get; set; }
        public string Website { get; set; }

        //quotation info (no QD info)

        public int InvoiceID { get; set; }
        public int InvoiceNumber { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public string Annotation { get; set; }
        public int CustomerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CellPhone { get; set; }
        public string EmailCustomer { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public int Box { get; set; }
        public int PostalCodeNumber { get; set; }
        public string Town { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactCellPhone { get; set; }
        public string VATnumberCustomer { get; set; }

        //Delivery Info
        public string DeliveryAddressInfo { get; set; }
        public string StreetNameNumberBox { get; set; }
        public string PostalCodeNumberTown { get; set; }


        //Details QD
        public int InvoiceDetailId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public double PriceExVAT { get; set; }
        public double Reprobel { get; set; }
        public double Bebat { get; set; }
        public double Recupel { get; set; }
        public double Auvibel { get; set; }
        public string Brand { get; set; }
        public string CategoryName { get; set; }
        public int VATValue { get; set; }
    }
}