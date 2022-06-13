using KurumsalWeb5.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using KurumsalWeb5.Models.Model;
using System.Net;

namespace KurumsalWeb5.Controllers
{
    public class HomeController : BaseController
    {
        private KurumsalDBContext db = new KurumsalDBContext();
        // GET: Home
        [Route("Home/Index")]
        [Route("")]
        [Route("Anasayfa")]
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(p => p.HizmetId);
            return View();
        }

        public ActionResult SliderPartial()
        {
            return View(db.Slider.ToList().OrderByDescending(p=> p.SliderId));
        }

        public ActionResult HizmetPartial()
        {
            return View(db.Hizmet.ToList().OrderByDescending(p=> p.HizmetId));
        }


        [Route("Urunler")]
        [Route("Home/Urunler")]
        public ActionResult Urunler()
        {
            db.Configuration.LazyLoadingEnabled = false;
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Product.Include("Category").ToList().OrderByDescending(p=>p.quantity));
        }

        [Route("Pastorders")]
        [Route("Home/Pastorders")]
        public ActionResult PastOrder()
        {
            db.Configuration.LazyLoadingEnabled = false;
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.PastOrder.Include("Category").ToList().OrderByDescending(p => p.PastOrderId));
        }

        public ActionResult Edit(int id)
        {
            var h = db.Product.Where(p => p.ProductId == id).FirstOrDefault();
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(h);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Product h)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var product = db.Product.Where(p => p.ProductId == id).SingleOrDefault();

                product.quantity = h.quantity;
                product.Defaultquantity = h.Defaultquantity;
                product.Limit = h.Limit;
                db.SaveChanges();
                return RedirectToAction("Urunler"); // güncelledikten sonra ana sayfaya döndürür.
            }

            return View(h);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Where(p => p.ProductId == id).SingleOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            product.quantity = 0;
            product.Defaultquantity = 0;
            product.Limit = 0;
            db.SaveChanges();
            //db.Hizmet.Remove(hizmet);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Urunler");
        }

        public ActionResult DeletePastOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PastOrder order = db.PastOrder.Where(p => p.PastOrderId == id).SingleOrDefault();
            if (order == null)
            {
                return HttpNotFound();
            }
            db.PastOrder.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Pastorders");
        }

        public ActionResult Details(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [Route("ReadBarcode")]
        public ActionResult ReadBarcode()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            //ViewBag.Message = TempData["Message"];
            return View();
        }


        [Route("Hakkimizda")]
        public ActionResult Hakkimizda()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hakkimizda.SingleOrDefault());
        }
        [Route("Hizmetlerimiz")]
        public ActionResult Hizmetlerimiz()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(p=>p.HizmetId));
        }
        [Route("Iletisim")]
        public ActionResult Iletisim()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Iletisim.SingleOrDefault());
        }
        [HttpPost]
        public ActionResult Iletisim(string name = null, string email = null, string konu = null, string mesaj = null)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (name != null && email != null)
            {

                WebMail.SmtpServer = "smtp-mail.outlook.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "egeumuttali@hotmail.com";
                WebMail.Password = "zuyoze33tali888";//sifre simdilik yok
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = false;
                //WebMail.Send("egeumuttali@hotmail.com",konu,email + "  "+ mesaj,"egeumuttali@hotmail.com");
                ViewBag.Uyari = "Mesajınız başarıyla gönderildi!";
            }
            else
            {
                ViewBag.Uyari = "Hata Oluştu Tekrar deneyiniz.";
            }

            return View();
        }
        [Route("BlogPost")]
        public ActionResult Blog(int Sayfa=1)
        {
            var language = this.Language_code();
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Blog.Include("Kategori").OrderByDescending(p => p.BlogId).Where(p=>p.language.Language_code == language).ToPagedList(Sayfa,5));
        }
        [Route("BlogPost/{kategoriad}/{id:int}")]
        public ActionResult KategoriBlog(int id,int sayfa=1) // blog sayfasında kategoriye tıklayınca o kategorideki blogların gelmesi için
        {
            var language = this.Language_code();
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var blog = db.Blog.Include("Kategori").OrderByDescending(p => p.BlogId).Where(p => p.KategoriId == id && p.language.Language_code == language).ToPagedList(sayfa,5);
            return View(blog); //seçilen kategoride belirlenen dilde bloglar gelmeli
        }

        public ActionResult HakkimizdaPartial()
        {
            return PartialView(db.Hakkimizda.SingleOrDefault());
        }

        public ActionResult BlogKategoriPartial()
        {
            return PartialView(db.Kategori.Include("Blogs").ToList().OrderByDescending(p=>p.KategoriAd));
        }

        public ActionResult BlogKayıtPartial()
        {
            var language = this.Language_code();
            return PartialView(db.Blog.Where(p=>p.language.Language_code == language).ToList().OrderByDescending(p=>p.BlogId));
        }

        [Route("BlogPost/{baslik}-{id:int}")]
        public ActionResult BlogDetay(int id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var blog = db.Blog.Include("Kategori").Include("Yorum").Where(p => p.BlogId == id).SingleOrDefault();
            return View(blog);
        }

        public JsonResult YorumYap(string adsoyad,string eposta,string icerik,int blogid)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (icerik ==null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Yorum.Add(new Yorum {AdSoyad=adsoyad,Eposta=eposta,Icerik=icerik,BlogId=blogid,Onay=false});
            db.SaveChanges();

            return Json(false,JsonRequestBehavior.AllowGet);
        }

        public ActionResult FooterPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
            ViewBag.Blog = db.Blog.ToList().OrderByDescending(p => p.BlogId);
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(p => p.HizmetId);
            return PartialView();
        }

        public ActionResult LanguagePartial()
        {
            return PartialView(db.Language.ToList().OrderByDescending(p => p.LanguageId));
        }

        public ActionResult MenuPartial()
        {
            var language = this.Language_code();
            return PartialView(db.Menu.Where(p => p.Language.Language_code == language).SingleOrDefault());
        }

        public ActionResult ChangeLanguage(string lang)
        {
            //var controller = (string)RouteData.Values["controller"];
            Session["lang"] = lang;
            return RedirectToAction("Index", new { language = lang });
        }

    }
}