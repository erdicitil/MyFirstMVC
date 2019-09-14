using MyFirstMVC.Data;
using MyFirstMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstMVC.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class ProjectsController : Controller
    {
        // GET: Admin/Projects
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var projects = db.Projects.Include("Category").ToList();
                return View(projects);
            }
                
        }
        public ActionResult Create()
        {
            var project = new Project();
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }
            return View(project);
        }
        [HttpPost]
        [ValidateInput(false)] // bu actiona html/script etiketleri artık gönderilebilir. 
        public ActionResult Create(Project project, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                //dosyayı upload etmeyi dene
                using(var db = new ApplicationDbContext())
                {
                    try
                    {
                        //yüklenen dosyanın adını entity'deki alana ata.
                        project.Photo = UploadFile(upload);
                    }
                    catch(Exception ex)
                    {//upload sırasında bir hata olursa View'de görünütülemek üzere hatayı değişekene ekle.
                        ViewBag.Error = ex.Message;
                        //hata oluştuğu için projeyi veritabanına eklemek yerine View'i tekrar göster metotttan çık.
                        ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        return View(project);
                    }
                   
                    db.Projects.Add(project);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }
            return View(project);
        }

        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var project = db.Projects.Where(x => x.Id == id).FirstOrDefault();
                if (project != null)
                {
                    ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    return View(project);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Project project, HttpPostedFileBase upload, string deletePhoto)
        {
            if (ModelState.IsValid)
            {
                using(var db = new ApplicationDbContext())
                {
                    try
                    {
                        project.Photo = UploadFile(upload);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = ex.Message;
                        ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        return View(project);
                    }
                    var oldProject = db.Projects.Where(x => x.Id == project.Id).FirstOrDefault();
                    if (oldProject != null)
                    {
                       
                        oldProject.Title = project.Title;
                        oldProject.Description = project.Description;
                        oldProject.Body = project.Body;
                        if (!string.IsNullOrEmpty(deletePhoto))
                        {
                            oldProject.Photo = null;
                        }
                        oldProject.CategoryId = project.CategoryId;
                        if (!string.IsNullOrEmpty(project.Photo))
                        {
                            oldProject.Photo = project.Photo;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }
            return View(project);
        }

        public ActionResult Delete(int id)
        {
            using(var db = new ApplicationDbContext())
            {
                var project = db.Projects.Where(x => x.Id == id).FirstOrDefault();
                if(project != null)
                {
                    db.Projects.Remove(project);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public string UploadFile(HttpPostedFileBase upload)
        {
            //yüklenmek istenen dosya var mı?
            if (upload != null && upload.ContentLength > 0)
            {
                //dosyanın uzantısını kontrol et.
                var extension = Path.GetExtension(upload.FileName).ToLower();
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".gif" || extension == ".png")
                {
                    //uzantı doğru ise dosyanın yükeleneceği Uploads var mı? Kontrol et.
                    if (Directory.Exists(Server.MapPath("~/Uploads")))
                    {
                        //dosya adındaki geçersiz karakterleri düzelt.
                        string fileName = upload.FileName.ToLower();
                        fileName = fileName.Replace("İ", "i");
                        fileName = fileName.Replace("Ş", "s").Replace("ı", "i").Replace("Ğ", "g").Replace("ğ", "g");
                        fileName = fileName.Replace("(", "");
                        fileName = fileName.Replace(")", "");
                        fileName = fileName.Replace(" ", "-");
                        fileName = fileName.Replace(",", "");
                        fileName = fileName.Replace("ö", "o");
                        fileName = fileName.Replace("ü", "u");
                        fileName = fileName.Replace("`", "");

                        //aynı isimde dosya olabilir diye dosya adının önüne zaman pulu ekliyoruz. 
                        /*Profesyonel siteler için guid kullanılabilir.32 haneli benzersiz bir sayı üretir. 
                        Detaylı bilgi http://www.ugurkizmaz.com/YazilimMakale-1414-GUID-Nedir----Globally-Unique-Identifier.aspx */

                        fileName = DateTime.Now.Ticks.ToString() + fileName;
                        //dosyayı Uploads dizinine yükle.
                        upload.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), fileName));
                        //yüklenen dosyanın adını geri döndür.
                        return fileName;
                    }
                    else
                    {
                        throw new Exception("Upload dizini mevcut değil");
                    }
                }
                else
                {
                    throw new Exception("Dosya uzantısı .jpg, .gif ya da .png olmalıdır.");
                }
            }
            return null;
        }
    }
}