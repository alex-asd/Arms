using ARMS.Data.Helpers;
using ARMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Search()
        {
            ViewBag.Message = "Enter the desired course name";

            return View(new List<Course>());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Search(string searchString)
        {
            ViewBag.SearchKey = searchString;
            var list = CourseHelper.SearchCoursesFor(searchString);

            return View(list);
        }
    }
}