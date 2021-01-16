using FXCamera.Models;
using FXCameraDbConText.Libraries;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace FXCameraDbConText.Areas.Administrator.Controllers
{
    [Authorize(Roles ="Admin, User")]
    public class UserController : Controller
    {
        FXCameraDbContext db = new FXCameraDbContext();
        // GET: Administrator/User

        public ActionResult Index(int? page)
        {

            int pageSize = 5;
            int pageIndex = page ?? 1;
            ViewBag.Id = "id";
            ViewBag.FirstName = "firstname";
            ViewBag.LastName = "lastname";
            var row = db.Users.OrderBy(m => m.Id).ToList().ToPagedList(pageIndex, pageSize);
            return View(row);
        }

        public ActionResult Page(string sort, string search, int? page = 1)
        {
            //Lấy thuộc tính sắp xếp hiện tại
            ViewBag.CurrentSort = sort;
            //Gán ngược thuộc tính sắp xếp qua ViewBag
            ViewBag.Id = sort == "id" ? "" : "id";
            ViewBag.FirstName = sort == "firstname" ? "firstname_desc" : "firstname";
            ViewBag.LastName = sort == "lastname" ? "lastname_desc" : "lastname";
            List<User> row = db.Users.ToList();
            ViewBag.CurrentSearch = "";
            //Truy vấn từ khóa tìm kiếm
            if (!String.IsNullOrEmpty(search))
            {
                //Giá trị cho textbox search
                ViewBag.CurrentSearch = search;
                row = db.Users.Where(m => m.FirstName.Contains(search.ToLower()) || m.LastName.Contains(search.ToLower())).ToList();
            }

            switch (sort)
            {
                case "firstname":
                    row = row.OrderBy(m => m.FirstName).ToList();
                    break;
                case "lastname":
                    row = row.OrderBy(m => m.LastName).ToList();
                    break;
                case "firstname_desc":
                    row = row.OrderByDescending(m => m.FirstName).ToList();
                    break;
                case "lastname_desc":
                    row = row.OrderByDescending(m => m.LastName).ToList();
                    break;
                case "id":
                    row = row.OrderByDescending(m => m.Id).ToList();
                    break;
                default:
                    row = row.OrderBy(m => m.Id).ToList();
                    break;

            }
            int pageSize = 5;
            int pageIndex = page ?? 1;


            return View("Index", row.ToList().ToPagedList(pageIndex, pageSize));
        }
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var row = db.Users.Find(id);
            bool result;
            if (row != null)
            {
                db.Users.Remove(row);
                db.Entry(row).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return Json(new { result, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult Status(int id)
        {
            var row = db.Users.Find(id);
            if (row != null)
            {
                row.Status = row.Status ? false : true;
                db.Entry(row).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new { result = row.Status, JsonRequestBehavior.AllowGet });

        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var row = db.Users.Find(id);
            if (row == null)
            {
                return HttpNotFound();
            }
            return View(row);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var row = db.Users.Find(user.Id);
                if (row != null)
                {
                    row.FirstName = user.FirstName;
                    row.LastName = user.LastName;
                    row.Email = user.Email;
                    row.Address = user.Address;
                    row.Phone = user.Phone;
                    row.Gender = user.Gender;
                    row.DateOfBirth = user.DateOfBirth;
                    row.Updated_At = DateTime.Now;
                    row.Updated_By = 1;
                    if (user.Files != null)
                    {
                        string file_name = Path.GetFileName(user.Files.FileName);
                        string path_upload = Path.Combine(Server.MapPath("~/Public/Images"), file_name);
                        user.Files.SaveAs(path_upload);
                        row.Avatar = file_name;
                    }
                    db.Entry(row).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = new Message("Thay đổi thông tin thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            TempData["message"] = new Message("Thay đổi thông tin thất bại", "warning");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChangePassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var row = db.Users.Find(id);
            if (row == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string Password, string ConfirmPassword, int id)
        {
            var row = db.Users.Where(m => m.Id == id).FirstOrDefault();
            if (row != null)
            {
                if (row.Password == MyString.HasPass(Password))
                {
                    row.Password = MyString.HasPass(ConfirmPassword);
                    db.Entry(row).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = new Message("Change password success!", "success");
                    return RedirectToAction("Detail", "User", new { @id = id });
                }
                else
                {
                    TempData["message"] = new Message("Current password wrong!", "error");
                    return View("ChangePassword");
                }
            }
            ViewBag.Id = id;
            return View("ChangePassword");
        }

    }
}