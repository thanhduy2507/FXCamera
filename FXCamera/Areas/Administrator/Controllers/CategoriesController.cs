using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FXCameraDbConText.Libraries;
using FXCamera.Models;
using PagedList;
using PagedList.Mvc;
namespace FXCameraDbConText.Areas.Administrator.Controllers
{
    public class CategoriesController : Controller
    {
        private FXCameraDbContext db = new FXCameraDbContext();


        public ActionResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            int pageSize = 10;
            var categories = db.Categories;
            return View(categories.ToList().ToPagedList(pageIndex, pageSize));
        }

        public ActionResult Page(string sort, string search, int? page)
        {
            ViewBag.CurentSort = sort;
            ViewBag.Id = String.IsNullOrEmpty(sort) ? "id" : "";
            ViewBag.Name = (sort == "name") ? "name_desc" : "name";
            List<Category> row = db.Categories.ToList();
            ViewBag.CurrentSearch = "";
            if (!String.IsNullOrEmpty(search))
            {
                row = row.Where(m => m.Name.Contains(search.ToLower())).ToList();
                ViewBag.CurrentSearch = search;
            }
            switch (sort)
            {

                case "name":
                    row = row.OrderBy(m => m.Name).ToList();
                    break;
                case "name_desc":
                    row = row.OrderByDescending(m => m.Name).ToList();
                    break;
                case "id":
                    row = row.OrderByDescending(m => m.Id).ToList();
                    break;
                default:
                    row = row.OrderBy(m => m.Id).ToList();
                    break;
            }
            int pageIndex = page ?? 1;
            int pageSize = 10;

            return View("Index", row.ToList().ToPagedList(pageIndex, pageSize));
        }
        [HttpPost]
        public JsonResult Status(int? id)
        {

            var row = db.Categories.Find(id);
            row.Status = (row.Status == true) ? false : true;
            db.Entry(row).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = row.Status, JsonRequestBehavior.AllowGet });
        }


        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.ParentID = new SelectList(db.Categories.Where(m => m.Status == true).ToList(), "Id", "Name", 0);

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Slug,ParentId,Orders,MetaKey,MetaDescription,Created_At,Created_By,ManufacturerId,Status")] Category category)
        {
            ViewBag.ParentID = new SelectList(db.Categories.Where(m => m.Status == true).ToList(), "Id", "Name", 0);

            if (ModelState.IsValid)
            {
                if (category.ParentId == null)
                {
                    category.ParentId = 0;
                }
                category.Slug = MyString.GenerateSlug(category.Name);
                category.Created_At = DateTime.Now;
                category.Created_By = 0;
                category.Status = true;
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["message"] = new Message("Add category successfuly", "success");
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = new Message("Add category failed", "error");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var row = db.Categories.Where(m => m.Status == true);
            ViewBag.ParentId = new SelectList(row, "Id", "Name", 0);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Slug,ParentId,Orders,MetaKey,MetaDescription,Created_At,Created_By,ManufacturerId,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var row = db.Categories.Where(m => m.Id == category.Id).FirstOrDefault();
                    row.ParentId = (category.ParentId == null) ? 0 : category.ParentId;
                    row.Updated_At = DateTime.Now;
                    row.Updated_By = 0;
                    row.Name = category.Name;
                    row.MetaKey = category.MetaKey;
                    row.MetaDescription = category.MetaDescription;
                    row.Orders = category.Orders;
                    row.Slug = MyString.GenerateSlug(category.Name);
                    db.Entry(row).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = new Message("Edit category successfuly", "success");
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    TempData["message"] = new Message("Edit category failed", "error");
                }

            }
            ViewBag.ParentID = new SelectList(db.Categories.Where(m => m.Status == true), "Id", "Name");
            return View();
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.Entry(category).State = EntityState.Deleted;
            db.SaveChanges();
            return Json( new { result=true, JsonRequestBehavior.AllowGet });
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
