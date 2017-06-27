using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OCTS.Models;

namespace OCTS.Controllers
{
    public class StudentController : Controller
    {
        private DBHelper dbHelper = DBHelper.getMyHelper();
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CourseInformation()
        {          
            return View();
        }
    }
}