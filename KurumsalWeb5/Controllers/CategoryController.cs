using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KurumsalWeb5.Models.DataContext;
using KurumsalWeb5.Models.Model;

namespace KurumsalWeb5.Controllers
{
    public class CategoryController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        // GET: Category
        public ActionResult Index()
        {
            return View(db.Category.ToList());
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Category category, HttpPostedFileBase PhotoURL)
        {
            if (ModelState.IsValid)
            {
                if (PhotoURL != null)
                    {

                    WebImage img = new WebImage(PhotoURL.InputStream);
                    FileInfo imginfo = new FileInfo(PhotoURL.FileName);

                    string logonName = PhotoURL.FileName;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Category/" + logonName);
                    category.PhotoURL = "/Uploads/Category/" + logonName;
            }

            db.Category.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
            }

            return View();
        }
    

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Uyarı("Güncellenecek Öğe Bulunamadı!");
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,Category category, HttpPostedFileBase PhotoURL)
        {
            if (ModelState.IsValid)
            {
                var h = db.Category.Where(p => p.CategoryId == id).SingleOrDefault();
                //var h = db.Hizmet.Find(id);

                if (PhotoURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(h.PhotoURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(h.PhotoURL));
                    }
                    WebImage img = new WebImage(PhotoURL.InputStream);
                    FileInfo imginfo = new FileInfo(PhotoURL.FileName);

                    string logonName = PhotoURL.FileName;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Category/" + logonName);
                    h.PhotoURL = "/Uploads/Category/" + logonName;
                }
                h.CategoryName = category.CategoryName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Category.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Category.Find(id);
            db.Category.Remove(category);
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
