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
using DeBrabander.ViewModels.Categories;

namespace DeBrabander.Controllers
{
    public class CategoriesController : Controller
    {
        private Context db = new Context();

        // GET: Categories
        public ActionResult Index()
        {
            List<CategoryIndexViewModel> categoryVMList = new List<CategoryIndexViewModel>();
            List<Category> categories = new List<Category>(db.Categories.ToList());

            foreach (var cat in categories)
            {
                CategoryIndexViewModel civm = new CategoryIndexViewModel();
                civm.CategoryId = cat.CategoryId;
                civm.CategoryName = cat.CategoryName;
                categoryVMList.Add(civm);
            }
            return View(categoryVMList);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            CategoryDetailsViewModel cdvm = new CategoryDetailsViewModel();
            cdvm.CategoryId = category.CategoryId;
            cdvm.CategoryName = category.CategoryName;
            

            return View(cdvm);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName")] CategoryCreateViewModel category)
        {
            if (ModelState.IsValid)
            {
                Category cat = new Category();
                cat.CategoryName = category.CategoryName;

                db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            CategoryEditViewModel cevm = new CategoryEditViewModel();
            cevm.CategoryId = category.CategoryId;
            cevm.CategoryName = category.CategoryName;

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(cevm);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName")] CategoryEditViewModel category)
        {
            if (ModelState.IsValid)
            {
                Category cat = new Category();
                cat.CategoryId = category.CategoryId;
                cat.CategoryName = category.CategoryName;

                db.Entry(cat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
