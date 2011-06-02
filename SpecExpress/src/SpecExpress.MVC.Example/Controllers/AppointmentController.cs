using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpecExpress.MVC.Example.Models;

namespace SpecExpress.MVC.Example.Controllers
{
    public class AppointmentController : Controller
    {
        //
        // GET: /Appointment/

        public ActionResult Index()
        {
            var model = new AppointmentModel();
            return View(model);
        }

         [HttpPost]
        public ActionResult Index(AppointmentModel model)
        {
            return View();
        }

    }
}
