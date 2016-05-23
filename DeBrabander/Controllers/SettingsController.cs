using DeBrabander.DAL;
using DeBrabander.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeBrabander.Controllers
{
    public class SettingsController : Controller
    {
        private Context db = new Context();
        // GET: Settings
        public ActionResult Index()
        {
            SettingsViewModel svm = new SettingsViewModel();
          
            return View(svm);
        }

       
    }
}