using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OCTS.Models;
using System.IO;
using Newtonsoft.Json;
namespace OCTS.Controllers
{
    public class TeacherController : Controller
    {
        DBHelper dbHelper = new DBHelper();
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ResourceManage()
        {
            return View();
        }

        public string uploadFile(HttpPostedFileBase file)
        {
            return "";
        }

        public string getResources()
        {
            string[] resultFile ;
            string[] resultDirectories;
            HttpCookie cookie = Request.Cookies["Account"];
            User u = dbHelper.findUser(cookie["userName"]);
            var severPath = this.Server.MapPath("/TeacherResource/" + u.userName+"/");
            Directory.CreateDirectory(severPath);
            try
            {
                resultFile = Directory.GetFiles(severPath);
                resultDirectories = Directory.GetDirectories(severPath);
            }catch(Exception e)
            {
                return "error";
            }
            string[] s = resultDirectories.Concat(resultFile).ToArray();
            return JsonConvert.SerializeObject(s);
        }
    }
}