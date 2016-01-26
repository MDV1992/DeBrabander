using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeBrabander.Models
{
    public class QuotationDetail
    {
        [Key]
        public int QuotationDetailId { get; set; }
        [DisplayName("Aantal")]
        public int Quantity { get; set; }
        public int QuotationId { get; set; }
        public virtual Quotation Quotation { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public double Total { get; set; }
        public void CalculateTotal()
        {
            Total = Product.PriceExVAT * Quantity;
            Quotation.TotalPrice = Quotation.QuotationDetail.Sum(x => x.Total);
        }

    }
}