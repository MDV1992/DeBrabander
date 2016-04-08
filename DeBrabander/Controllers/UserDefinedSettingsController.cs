using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeBrabander.DAL;
using DeBrabander.Models;

namespace DeBrabander.Controllers
{
    public class UserDefinedSettingsController : Controller
    {
        private Context db = new Context();
        // GET: UserDefinedSetting/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDefinedSetting userDefinedSetting = db.UserDefinedSettings.Find(id);
            if (userDefinedSetting == null)
            {
                return HttpNotFound();
            }
            return View(userDefinedSetting);
        }

        // POST: UserDefinedSetting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IndexResultLength,DetailsResultLength,ExpirationDateLength")] UserDefinedSetting userDefinedSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userDefinedSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Settings");
            }
            return View(userDefinedSetting);
        }

       
    }
}
