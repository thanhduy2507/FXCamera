using FXCamera.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FXCameraDbConText.Areas.Administrator.Controllers
{
    public class AdminController : Controller
    {
        FXCameraDbContext db = new FXCameraDbContext();
        // GET: Administrator/Admin
        public AdminController()
        {
            //if (System.Web.HttpContext.Current.Session["UserAdmin"] == null)
            //{
            //    System.Web.HttpContext.Current.Response.Redirect("~/Admin/login");

            //}
        }
      
        public ActionResult Index()
        {
            return View();
        }
       

    }
}
