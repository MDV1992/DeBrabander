using DeBrabander.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeBrabander.ViewModels.Categories
{
    public class CategoryEditViewModel
    {
        public int CategoryId { get; set; }

        [DisplayName("Categorie")]
        public string CategoryName { get; set; }
    }

}