using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpecExpress.MVC.Example.Models;

namespace SpecExpress.MVC.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var meeting = new Meeting() { StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(1), MinTitleLength = 4};
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View(meeting);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var meeting = new Meeting();

            if (TryUpdateModel(meeting))
            {
                return RedirectToAction("Index");
            }

            return View(meeting);
        }


        public ActionResult About()
        {
            return View();
        }
    }
}
