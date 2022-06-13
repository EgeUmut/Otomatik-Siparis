using KurumsalWeb5.Models.DataContext;
using KurumsalWeb5.Models.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb5.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        private KurumsalDBContext db = new KurumsalDBContext();
        public ActionResult Index()
        {
            db.Configuration.LazyLoadingEnabled = false; // farklı tablodan öğe alırken böyle bir şey yapman lazım
            return View(db.Blog.Include("Kategori").Include("Language").ToList().OrderByDescending(p => p.BlogId));
        }

        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd");
            ViewBag.LanguageId = new SelectList(db.Language, "LanguageId", "Baslik");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Blog blog, HttpPostedFileBase ResimURL)
        {

            if (ModelState.IsValid)
            {
                
                if (ResimURL != null)
                {
                    
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string logonName = ResimURL.FileName;
                    img.Resize(600, 400);
                    img.Save("~/Uploads/Blog/" + logonName);
                    blog.ResimURL = "/Uploads/Blog/" + logonName;
                }
                db.Blog.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Uyarı("Güncellenecek Öğe Bulunamadı!");
            }
            Blog blog = db.Blog.Where(p => p.BlogId == id).SingleOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd", blog.KategoriId);
            ViewBag.LanguageId = new SelectList(db.Language, "LanguageId", "Baslik",blog.LanguageId);
            return View(blog);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Blog blog, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                var b = db.Blog.Where(p => p.BlogId == id).SingleOrDefault();
                //var h = db.Hizmet.Find(id);

                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(b.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(b.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string logonName = ResimURL.FileName;
                    img.Resize(600, 400);
                    img.Save("~/Uploads/Blog/" + logonName);
                    b.ResimURL = "/Uploads/Blog/" + logonName;
                }
                b.Baslik = blog.Baslik;
                b.Icerik = blog.Icerik;
                b.KategoriId = blog.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blog.Where(p => p.BlogId == id).SingleOrDefault();
            if (blog == null)
            {
                return HttpNotFound();
            }
            //db.Hizmet.Remove(hizmet);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            return View(blog);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComfirm(int id)
        {
            Blog blog = db.Blog.Find(id);
            db.Blog.Remove(blog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blog.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }


    }
}