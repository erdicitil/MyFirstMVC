using MyFirstMVC.Data;
using MyFirstMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Görüş ve önerileriniz için lütfen aşağıdaki formu doldurunuz.";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    //TODO: mail gönder
                    System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
                    mailMessage.From = new System.Net.Mail.MailAddress("erdicitil1@gmail.com", "Blog");
                    mailMessage.Subject = "İletişim Formu: " + model.firstName + " " + model.firstName;
                    //gönderilecek mail
                    mailMessage.To.Add("erdicitil1@gmail.com");

                    string body;
                    body = "Ad Soyad: " + model.firstName + " " + model.firstName + "<br />";
                    body += "Telefon: " + model.Telephone + "<br />";
                    body += "E-posta: " + model.Email + "<br />" + "<br/>";
                    body += "Tarih: " + DateTime.Now.ToString("dd MMMM yyyy") + "<br />";
                    body += "Mesaj: " + model.Message + "<br />";

                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = body;

                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                    //gönderen mail
                    smtp.Credentials = new System.Net.NetworkCredential("erdicitil1@gmail.com", "");
                    smtp.EnableSsl = true;
                    smtp.Send(mailMessage);
                    ViewBag.Message = "Mesajınız gönderildi. Teşekkür ederiz.";
                }
                catch(Exception ex)
                {
                    ViewBag.Error = "Form gönderimi başarısız oldu. Lütfen daha sonra tekrar deneyiniz.";
                }
            }
            return View(model);
        }

        public ActionResult Projects()
        {
            using (var db = new ApplicationDbContext())
            {
                var projects = db.Projects.ToList();
                return View(projects);
            }

           
        }

        public ActionResult TermProlicy()
        {
            return View();
        }

        public ActionResult PrivacyProlicy()
        {
            return View();
        }

        public ActionResult Kvkk()
        {
            return View();
        }
    }
}