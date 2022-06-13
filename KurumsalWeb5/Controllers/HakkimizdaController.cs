using KurumsalWeb5.Models.DataContext;
using KurumsalWeb5.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWeb5.Controllers
{
    public class HakkimizdaController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Hakkimizda
        public ActionResult Index()
        {
            var h = db.Hakkimizda.ToList();
            return View(h);
        }

        
        public ActionResult Edit(int id)
        {
            var h = db.Hakkimizda.Where(p => p.HakkimizdaId == id).FirstOrDefault();
            return View(h);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id ,Hakkimizda h)
        {
            
            if (ModelState.IsValid)
            {
                var hakkimizda = db.Hakkimizda.Where(p => p.HakkimizdaId == id).SingleOrDefault();

                hakkimizda.Aciklama = h.Aciklama;
                db.SaveChanges();
                return RedirectToAction("Index"); // güncelledikten sonra ana sayfaya döndürür.
            }



            return View(h);
        }
    }
}