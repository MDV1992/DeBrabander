using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeBrabander.Models;

namespace DeBrabander.ViewModels.Settings
{
    public class SettingsViewModel
    {
        public Category category = new Category();
        public List<Category> categorys = new List<Category>();
    }
}