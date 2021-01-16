using FXCamera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FXCameraDbConText.Libraries;
using System.Web.Security;

namespace FXCameraDbConText.Areas.Administrator.Controllers
{
    public class AuthController : Controller
    {
        FXCameraDbContext db = new FXCameraDbContext();
        // GET: Administrator/Auth
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            var row = db.Users.Where(m => m.UserName == user.UserName).FirstOrDefault();
            if (row != null)
            {
                if (row.Password == MyString.HasPass(user.Password))
                {
                    if (row.Status == true)
                    {
                        TempData["message"] = new Message("Login success!", "success");
                        FormsAuthentication.SetAuthCookie(user.UserName,false);
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        TempData["message"] = new Message("Accout is locked!", "error");
                        return View();
                    }
                }
                else
                {
                    TempData["message"] = new Message("Password wrong!", "error");
                    return View();
                }
            }
            else
            {
                TempData["message"] = new Message("Account is not exist!", "error");
                return View();
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User user)
        {
            var row = db.Users.Where(m => m.UserName == user.UserName).FirstOrDefault();
            if (row != null)
            {
                TempData["message"] = new Message("Tài khoản đã tồn tại","error");
                return View();
            }
            if (ModelState.IsValid)
            {
                user.Created_At = DateTime.Now;
                user.Password = MyString.HasPass(user.Password);
                user.RoleId = 3;
                user.Status = true;
                db.Users.Add(user);
                db.SaveChanges();
                TempData["message"] = new Message("Tạo tài khoản thành công","success");
                return RedirectToAction("Index", "User");
            }
            return View();
        }
    }
}