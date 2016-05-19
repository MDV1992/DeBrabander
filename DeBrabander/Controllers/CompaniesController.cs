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

using System.IO;

namespace DeBrabander.Controllers
{
    public class CompaniesController : Controller
    {
        private Context db = new Context();

        // GET: Companies
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyId,CompanyName,Street, PostalCode, District,Phone,Mobile,Email,Country,VatNumber,Iban,BIC,Website")] Company company, HttpPostedFileBase upload)
        {
            
            if (ModelState.IsValid)
            {
                
            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Settings");
            }
            return View(company);
        }


    }
}
