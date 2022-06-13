using KurumsalWeb5.Models.DataContext;
using KurumsalWeb5.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb5.Controllers
{
    public class HizmetController : Controller
    {
        // GET: Hizmet
        KurumsalDBContext db = new KurumsalDBContext();
        public ActionResult Index()
        {
            var hizmet = db.Hizmet.ToList();
            return View(hizmet);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Hizmet hizmet,HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                
                if (ResimURL != null)
                {
                    
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string logonName = ResimURL.FileName;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Hizmet/" + logonName);
                    hizmet.ResimURL = "/Uploads/Hizmet/" + logonName;
                }

                db.Hizmet.Add(hizmet);
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
            Hizmet hizmet = db.Hizmet.Find(id);
            if (hizmet == null)
            {
                return HttpNotFound();
            }          
            return View(hizmet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int? id ,Hizmet hizmet, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid)
            {
                var h = db.Hizmet.Where(p => p.HizmetId == id).SingleOrDefault();
                //var h = db.Hizmet.Find(id);

                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(h.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(h.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);

                    string logonName = ResimURL.FileName;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Hizmet/" + logonName);
                    h.ResimURL = "/Uploads/Hizmet/" + logonName;
                }
                h.Baslik = hizmet.Baslik;
                h.Aciklama = hizmet.Aciklama;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hizmet);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hizmet hizmet = db.Hizmet.Where(p => p.HizmetId == id).SingleOrDefault();
            if (hizmet == null)
            {
                return HttpNotFound();
            }
            //db.Hizmet.Remove(hizmet);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            return View(hizmet);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComfirm(int id)
        {
            Hizmet hizmet = db.Hizmet.Find(id);
            db.Hizmet.Remove(hizmet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}