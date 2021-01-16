using FXCamera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.IO;
using FXCameraDbConText.Libraries;

namespace FXCameraDbConText.Areas.Administrator.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductsController : Controller
    {
        // GET: Product
        private FXCameraDbContext db = new FXCameraDbContext();
        public ActionResult Index(int page = 1)
        {
            ViewBag.Id = "id";
            ViewBag.Name = "name";
            ViewBag.Price = "price";
            int pageIndex = page;
            int pageSize = 10;
            var row = db.Products;
            return View(row.OrderBy(m => m.Id).ToList().ToPagedList(pageIndex, pageSize));
        }
        public ActionResult Page(string sort, string search, int? page = 1)
        {
            //Lấy thuộc tính sắp xếp hiện tại
            ViewBag.CurrentSort = sort;
            //Gán ngược thuộc tính sắp xếp qua ViewBag
            ViewBag.Id = sort == "id" ? "" : "id";
            ViewBag.Name = sort == "name" ? "name_desc" : "name";
            ViewBag.Price = sort == "price" ? "price_desc" : "price";
            List<Product> row = db.Products.ToList();
            ViewBag.CurrentSearch = "";
            //Truy vấn từ khóa tìm kiếm
            if (!String.IsNullOrEmpty(search))
            {
                //Giá trị cho textbox search
                ViewBag.CurrentSearch = search;
                row = db.Products.Where(m => m.Name.Contains(search.ToLower())).ToList();
            }

            switch (sort)
            {
                case "name":
                    row = row.OrderBy(m => m.Name).ToList();
                    break;
                case "price":
                    row = row.OrderBy(m => m.Price).ToList();
                    break;
                case "name_desc":
                    row = row.OrderByDescending(m => m.Name).ToList();
                    break;
                case "price_desc":
                    row = row.OrderByDescending(m => m.Price).ToList();
                    break;
                case "id":
                    row = row.OrderByDescending(m => m.Id).ToList();
                    break;
                default:
                    row = row.OrderBy(m => m.Id).ToList();
                    break;

            }
            int pageSize = 10;
            int pageIndex = page ?? 1;
            return View("Index", row.ToList().ToPagedList(pageIndex, pageSize));
        }
        //public ActionResult Page(string sortOrder, string searchString, string currentFilter, int? page)
        //{
        //    ViewBag.CurentSort = sortOrder;
        //    ViewBag.Id = String.IsNullOrEmpty(sortOrder) ? "id" : "";
        //    ViewBag.Name = (sortOrder == "name") ? "name_desc" : "name";
        //    ViewBag.Price = (sortOrder == "price") ? "price_desc" : "price";
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }
        //    // return values for textbox search
        //    ViewBag.CurrentFilter = searchString;
        //    var row = db.Products.ToList();

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        row = row.Where(m => m.Name.Contains(searchString) || m.MetaKey.Contains(searchString)).ToList();
        //    }
        //    switch (sortOrder)
        //    {

        //        case "name":
        //            row = row.OrderBy(m => m.Name).ToList();
        //            break;
        //        case "name_desc":
        //            row = row.OrderByDescending(m => m.Name).ToList();
        //            break;
        //        case "price":
        //            row = row.OrderBy(m => m.Price).ToList();
        //            break;
        //        case "price_desc":
        //            row = row.OrderByDescending(m => m.Price).ToList();
        //            break;
        //        case "id":
        //            row = row.OrderByDescending(m => m.Id).ToList();
        //            break;
        //        default:
        //            row = row.OrderBy(m => m.Id).ToList();
        //            break;
        //    }
        //    int pageIndex = page ?? 1;
        //    int pageSize = 10;

        //    return View("Index", row.ToPagedList(pageIndex, pageSize));
        //}
        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {

            ViewBag.CategoriesID = new SelectList(db.Categories.Where(m => m.Status == true).ToList(), "Id", "Name", 0);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers.ToList(), "Id", "Name", 0);
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            ViewBag.CategoriesID = new SelectList(db.Categories.Where(m => m.Status == true).ToList(), "Id", "Name", 0);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers.ToList(), "Id", "Name", 0);
            if (ModelState.IsValid)
            {
                if (product.Name != null)
                {
                    product.Slug = Libraries.MyString.GenerateSlug(product.Name);
                }
                if (product.Files != null)
                {
                    string file_name = Path.GetFileNameWithoutExtension(product.Files.FileName);
                    string file_extension = Path.GetExtension(product.Files.FileName);
                    file_name = file_name + DateTime.Now.ToString("yyyy-MM-dd") + file_extension;
                    string path_upload = Path.Combine(Server.MapPath("~/Public/Images"), file_name);
                    product.Files.SaveAs(path_upload);
                    product.Images = file_name;
                }
                product.Created_At = DateTime.Now;
                product.Created_By = 0;
                product.Status = true;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Product row = db.Products.Find(id);
            if (row == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriesID = new SelectList(db.Categories.Where(m => m.Status == true).ToList(), "Id", "Name", 0);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers.ToList(), "Id", "Name", 0);
            return View(row);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product row)
        {
            Product product = db.Products.Find(row.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    product.Name = row.Name;
                    product.CategoryId = row.CategoryId;
                    product.ManufacturerId = row.ManufacturerId;
                    product.Slug = MyString.GenerateSlug(row.Name);
                    product.Detail = row.Detail;
                    product.MetaKey = row.MetaKey;
                    product.MetaDescription = row.MetaDescription;
                    product.Number = row.Number;
                    product.Price = row.Price;
                    product.PriceSale = row.PriceSale;
                    product.Updated_At = DateTime.Now;
                    product.Updated_By = 0;
                    if (row.Files != null)
                    {
                        string file_name = Path.GetFileNameWithoutExtension(row.Files.FileName);
                        string file_extension = Path.GetExtension(row.Files.FileName);
                        file_name = file_name + "-" + DateTime.Now.ToString("yyyy-MM-dd") + file_extension;
                        string path_upload = Path.Combine(Server.MapPath("~/Public/Images"), file_name);
                        row.Files.SaveAs(path_upload);
                        product.Images = file_name;

                    }
                    db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Sửa thất bại");
                }
            }
            ViewBag.CategoriesID = new SelectList(db.Categories.Where(m => m.Status == true).ToList(), "Id", "Name", 0);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers.ToList(), "Id", "Name", 0);
            return View("Edit", product);
        }
        [HttpPost]
        public ActionResult Status(int id)
        {

            var row = db.Products.Find(id);
            if (row == null)
            {
                return Json(new { error = "Thất bại" });
            }
            row.Status = (row.Status == true) ? false : true;
            db.Entry(row).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new { result = row.Status }, JsonRequestBehavior.AllowGet);


        }
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var row = db.Products.Find(id);
            if (row == null)
            {
                return HttpNotFound();
            }
            try
            {
                db.Products.Remove(row);
                db.Entry(row).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }


    }
}
