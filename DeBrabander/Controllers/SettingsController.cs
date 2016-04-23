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
            var categoryList = from c in db.Categories select c;
            svm.categorys = categoryList.ToList();
            return View(svm);
        }

        //[HttpPost]
        //public ActionResult UploadImages(HttpPostedFileBase[] uploadImages)
        //{
        //    if (uploadImages.Count() <= 1)
        //    {
        //        return RedirectToAction("BrowseImages");
        //    }

        //    foreach (var image in uploadImages)
        //    {
        //        if (image.ContentLength > 0)
        //        {
        //            byte[] imageData = null;
        //            using (var binaryReader = new BinaryReader(image.InputStream))
        //            {
        //                imageData = binaryReader.ReadBytes(image.ContentLength);
        //            }
        //            var headerImage = new HeaderImage
        //            {
        //                ImageData = imageData,
        //                ImageName = image.FileName,
        //                IsActive = true
        //            };
        //            imageRepository.AddHeaderImage(headerImage);
        //        }
        //    }
        //    return RedirectToAction("BrowseImages");
        //}
    }
}