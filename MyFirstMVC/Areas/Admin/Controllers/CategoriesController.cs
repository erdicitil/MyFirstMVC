using MyFirstMVC.Data;
using MyFirstMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyFirstMVC.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class CategoriesController : Controller
    {
        // GET: Admin/Categories
        public ActionResult Index()
        {
            using(var db = new ApplicationDbContext())
            {
                var categories = db.Categories.ToList();
                return View(categories);
            }
            
        }

        public ActionResult Create()
        {
            var categories = new Category();
            return View(categories);
        }
        [HttpPost]
        
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }

        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var category = db.Categories.Where(x => x.Id == id).FirstOrDefault();
                if (category != null)
                {
                    return View(category);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    var oldCategory = db.Categories.Where(x => x.Id == category.Id).FirstOrDefault();
                    if (oldCategory != null)
                    {
                        oldCategory.Name = category.Name;
                        oldCategory.Description = category.Description;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(category);
        }

        public ActionResult Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var category = db.Categories.Where(x => x.Id == id).FirstOrDefault();
                var projects = db.Projects.Where(x => x.CategoryId == id).ToList();
                if (category != null)
                {
                    foreach (var item in projects)
                    {
                        item.CategoryId = null;
                    }
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
    }
}