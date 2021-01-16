using FXCamera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace FXCameraDbConText.Areas.Administrator.Controllers
{
    public class OrdersController : Controller
    {
        FXCameraDbContext db = new FXCameraDbContext();
        // GET: Administrator/Orders
        public ActionResult Index(int? page)
        {
            var row = db.Orders.ToList();
            int pageSize = 10;
            int pageIndex = page ?? 1;
            ViewBag.Id = "id";
            ViewBag.Name = "name";
            ViewBag.CreatedDay = "created-day";
            ViewBag.ExportDay = "export-day";
            return View(row.OrderBy(m => m.Id).ToList().ToPagedList(pageIndex, pageSize));
        }
        public ActionResult Page(string sort, string search, int? page)
        {
            ViewBag.CurrentSort = sort;
            ViewBag.Id = sort == "id" ? "" : "id";
            ViewBag.Name =sort== "name"?"name_desc":"name";
            ViewBag.CreatedDay = sort == "created-day" ? "created-day-desc" : "created-day";
            ViewBag.ExportDay = sort == "export-day" ? "export-day-desc" : "export-day";
            ViewBag.CurrentSearch = "";
            List<Order> row = db.Orders.ToList();
            if (!String.IsNullOrEmpty(search))
            {
                ViewBag.CurrentSearch = search;
                row = row.Where(m => m.Users.FirstName.Contains(search.ToLower())||m.Users.LastName.Contains(search.ToLower())).ToList();
            }
            switch (sort)
            {
                case "name":
                    row = row.OrderBy(m => m.Users.FirstName).ToList();
                    break;
                case "name_desc":
                    row = row.OrderByDescending(m => m.Users.FirstName).ToList();
                    break;
                case "created-day":
                    row = row.OrderBy(m => m.CreatedDay).ToList();
                    break;
                case "created-day-desc":
                    row = row.OrderByDescending(m => m.CreatedDay).ToList();
                    break;
                case "export-day":
                    row = row.OrderBy(m => m.ExportDay).ToList();
                    break;
                case "export-day-desc":
                    row = row.OrderByDescending(m => m.ExportDay).ToList();
                    break;
                default:
                    row = row.OrderBy(m => m.Id).ToList();
                    break;
            }
            int pageSize = 10;
            int pageIndex=page?? 1;
            return View("Index",row.ToList().ToPagedList(pageIndex, pageSize));
        }
    }
}