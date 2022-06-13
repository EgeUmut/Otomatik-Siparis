using KurumsalWeb5.Models.DataContext;
using KurumsalWeb5.Models.Model;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb5.Controllers
{
    public class ReadController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        [HttpPost]
        [Route("api/ScanBarcode")]
        public ActionResult ScanBarcode(Product product)
        {
            
            if (ModelState.IsValid)
            {
                string barcode = product.BarcodeNumber;
                var model = db.Product.Where(p => p.BarcodeNumber == barcode).SingleOrDefault();
                if (model != null)
                {
                    model.quantity -= 1;
                    if (CheckLimiter(model))
                    {
                        //sipariş ve mail kısmı
                        int sayı = model.Defaultquantity - model.quantity;
                        model.quantity = model.Defaultquantity; // ürün sayısını default a getirdik  (sipariş demosu)

                        try
                        {
                            var message = new MimeMessage();
                            message.To.Add(new MailboxAddress("Ege Umut", "ege_tali@hotmail.com"));
                            message.From.Add(new MailboxAddress("Otomatik Sipariş!", "selampankasudde@hotmail.com"));
                            message.Subject = "Otomatik Sipariş Devreye Girdi";
                            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                            {
                                Text = "Otomatik sipariş devreye girmiştir! " + sayı + " adet " + model.ProductName + " alınmıştır. Toplam Ücret: " + model.UnitPrice * sayı + "₺",
                            };

                            using (var emailClient = new SmtpClient())
                            {

                                emailClient.Connect("smtp-mail.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                                //emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                                emailClient.Authenticate("selampankasudde@hotmail.com", "zuyoze33"); // bloke olmuş olabilir.
                                emailClient.Send(message);
                                emailClient.Disconnect(true);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw e;
                        }

                        CreatePastOrder(model, sayı);
                        db.SaveChanges();
                        TempData["Message"] = "Ürün Okutulup limite gelinmiştir lütfen mailinizi kontrol edin";
                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                    }
                    db.SaveChanges();
                    TempData["Message"] = "Ürün Okutulmutur";
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

                var item = CheckBarcodeDifference(product);
                if (product != item)
                {
                    if (item != null)
                    {
                        item.quantity -= 1;
                        if (CheckLimiter(item))
                        {
                            //sipariş ve mail kısmı
                            int sayı = item.Defaultquantity - item.quantity;
                            item.quantity = item.Defaultquantity; // ürün sayısını default a getirdik  (sipariş demosu)

                            try
                            {
                                var message = new MimeMessage();
                                message.To.Add(new MailboxAddress("Ege Umut", "egeumuttali@hotmail.com"));
                                message.From.Add(new MailboxAddress("Otomatik Sipariş!", "selampankasudde@hotmail.com"));
                                message.Subject = "Otomatik Sipariş Devreye Girdi";
                                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                                {
                                    Text = "Otomatik sipariş devreye girmiştir! " + sayı + " adet " + item.ProductName + " alınmıştır. Toplam Ücret: " + item.UnitPrice * sayı + "₺",
                                };

                                using (var emailClient = new SmtpClient())
                                {

                                    emailClient.Connect("smtp-mail.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                                    //emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                                    emailClient.Authenticate("selampankasudde@hotmail.com", "zuyoze33"); // bloke olmuş olabilir.
                                    emailClient.Send(message);
                                    emailClient.Disconnect(true);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw e;
                            }

                            //WebMail.SmtpServer = "smtp-mail.outlook.com";
                            //WebMail.EnableSsl = true;
                            //WebMail.UserName = "egeumuttali@hotmail.com";
                            //WebMail.Password = "";//sifre simdilik yok 
                            //WebMail.SmtpPort = 587;
                            //WebMail.SmtpUseDefaultCredentials = false;

                            //WebMail.Send("egeumuttali@hotmail.com", "Otomatik Sipariş"
                            //    , "Otomatik sipariş devreye girmiştir! " + sayı + " adet " + model.ProductName + " alınmıştır. Toplam Ücret: " + model.UnitPrice * sayı + "₺", "egeumuttali@hotmail.com");

                            CreatePastOrder(item, sayı);
                            db.SaveChanges();
                            TempData["Message"] = "Ürün Okutulup limite gelinmiştir lütfen mailinizi kontrol edin";
                            return new HttpStatusCodeResult(HttpStatusCode.OK);
                        }
                        db.SaveChanges();
                        TempData["Message"] = "Ürün Okutulmutur";
                        return new HttpStatusCodeResult(HttpStatusCode.OK);
                    }
                }
                else
                {
                TempData["Message"] = "Böyle bir ürün kayıtlı değildir";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            TempData["Message"] = "Başarısız";
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public Boolean CheckLimiter(Product product)
        {
            int quantity = product.quantity;
            int defaultquantity = product.Defaultquantity;
            int limit = product.Limit;

            if ((defaultquantity*limit)/100 > quantity)
            {
                //sipariş verilecek
                return true;
            }
            //sipariş verilmeyecek
            return false;
        }

        public Product CheckBarcodeDifference(Product product)
        {
            
            List<Product> products = db.Product.ToList();

            foreach (var item in products)
            {
                int diff = 0;
                for (int i = 0; i < item.BarcodeNumber.Length; i++)
                {
                    char a = product.BarcodeNumber[i];
                    char b = item.BarcodeNumber[i];
                    if (a != b)
                    {
                        diff++;
                    }
                    if (diff >1)
                    {
                        i = 30;
                    }
                    if (diff < 2 && i == item.BarcodeNumber.Length-1)
                    {
                        return item;
                    }
                }
            }
            return product;
        }

        public void CreatePastOrder(Product product,int sayi)
        {
            db.PastOrder.Add(new PastOrder { CategoryId=product.CategoryId,
                ProductName=product.ProductName,
                quantity=sayi,
                PriceSum=sayi*product.UnitPrice,
                Time=DateTime.Now});
            db.SaveChanges();
        }
    }
}