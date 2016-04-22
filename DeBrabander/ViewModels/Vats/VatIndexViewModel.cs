﻿using DeBrabander.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeBrabander.ViewModels.VATs
{
    public class VatIndexViewModel
    {
        public int VATPercId { get; set; }

        [DisplayName("BTW")]
        public double VATValue { get; set; }

        public VAT vat = new VAT();
    }

}