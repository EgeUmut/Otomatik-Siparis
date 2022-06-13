using KurumsalWeb5.Models;
using KurumsalWeb5.Models.DataContext;
using KurumsalWeb5.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb5.Controllers
{
    public class AdminController : Controller
    {
        //test
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Admin
        [Route("yonetimpaneli")]
        public ActionResult Index()
        {
            var sorgu = db.Kategori.ToList();
            ViewBag.YorumOnay = db.Yorum.Where(p => p.Onay == false).Count();
            ViewBag.BlogSay = db.Blog.Count();
            ViewBag.KategoriSay = db.Kategori.Count();
            ViewBag.HizmetSay = db.Hizmet.Count();
            ViewBag.YorumSay = db.Yorum.Count();
            ViewBag.UrunSay = db.Product.Count();
            return View(sorgu);
        }
        [Route("yonetimpaneli/giris")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult Login(Admin admin)
        {
            var md5password = Crypto.Hash(admin.Sifre, "MD5");
            var login = db.Admin.Where(p => p.Eposta == admin.Eposta).SingleOrDefault();

            if (login == null)
            {
                ViewBag.Uyari="Böyle bir kullanıcı bulunmamaktadır";
                return View(admin);
            }

            if (login.Eposta == admin.Eposta && login.Sifre == md5password)
            {
                Session["adminid"] = login.AdminId;
                Session["eposta"] = login.Eposta;
                Session["yetki"] = login.Yetki;
                return RedirectToAction("Index");

            }
            ViewBag.Uyari="Kullanıcı Adı ya da Şifre Yanlış";
            return View(admin);
        }

        public ActionResult RememberMe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RememberMe(string eposta)
        {
            var mail = db.Admin.Where(p => p.Eposta == eposta).SingleOrDefault();
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (mail != null)
            {
                Random rnd = new Random();
                int yeniSifre = rnd.Next();
                Admin sifre = new Admin();
                mail.Sifre = Crypto.Hash(Convert.ToString(yeniSifre), "MD5");
                db.SaveChanges();

                WebMail.SmtpServer = "smtp-mail.outlook.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "egeumuttali@hotmail.com";
                WebMail.Password = ""; //sifre simdilik yok
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = false;
                WebMail.Send(eposta, "admin panel giriş şifreniz", " şifreniz = "+ Convert.ToString(yeniSifre),  "egeumuttali@hotmail.com");
                ViewBag.Uyari = "Mesajınız başarıyla gönderildi!";
            }
            else
            {
                ViewBag.Uyari = "Hata Oluştu Tekrar deneyiniz.";
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session["adminid"] = null;
            Session["eposta"] = null;
            Session.Abandon();
            return RedirectToAction("Login","Admin");

            return View();
        }

        public ActionResult Adminler()
        {
            return View(db.Admin.ToList());
        }
        
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Admin admin ,string sifre,string eposta)
        {
            if (ModelState.IsValid)
            {
                admin.Sifre = Crypto.Hash(sifre, "MD5");
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Adminler","Admin");

            }
            return View(admin);
        }

        public ActionResult Edit(int id)
        {
            var admin = db.Admin.Where(p => p.AdminId == id).SingleOrDefault();
            return View(admin);
        }
        [HttpPost]
        public ActionResult Edit(int id,Admin admin)
        {

            if (ModelState.IsValid)
            {
                var tmp = db.Admin.Where(p => p.AdminId == id).SingleOrDefault();
                tmp.Sifre = Crypto.Hash(admin.Sifre, "MD5");
                tmp.Eposta = admin.Eposta;
                tmp.Yetki = admin.Yetki;
                db.SaveChanges();
                return RedirectToAction("Adminler", "Admin");
                
            }
            return View(admin);
        }
        public ActionResult Delete(int id)
        {
            var admin = db.Admin.Where(p => p.AdminId == id).SingleOrDefault();
            if (admin != null)
            {
                db.Admin.Remove(admin);
                db.SaveChanges();
                return RedirectToAction("Adminler", "Admin");
            }
            return View();
        }
    }
}