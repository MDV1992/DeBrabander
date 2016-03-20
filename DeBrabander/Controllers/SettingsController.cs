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
        // GET: Settings
        public ActionResult Index()
        {
            return View();
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