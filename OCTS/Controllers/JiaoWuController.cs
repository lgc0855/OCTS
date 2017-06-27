using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OCTS.Models;

namespace OCTS.Controllers
{
    public class JiaoWuController : Controller
    {
        private DBHelper dbHelper = DBHelper.getMyHelper();
        // GET: JiaoWu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserInformation()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            List<User> list = dbHelper.getUsers();
            string userName = accountCookie["userName"];
            foreach(User user in list)
            {
                if (userName == user.userId)
                {
                    ViewData["userid"] = user.userId;
                    ViewData["username"] = user.userName;
                    ViewData["usersex"] = user.userSex;
                    ViewData["useremail"] = user.userEmail;
                    ViewData["userphone"] = user.userPhone;
                }
            }
            return View();
        }
        public ActionResult Update(string name,string email,string phone,string password)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            List<User> list = dbHelper.getUsers();
            string userName = accountCookie["userName"];
            foreach (User user in list)
            {
                if (userName == user.userId)
                {
                    dbHelper.setUsers(userName, name, email, phone, password);
                    return RedirectToAction("UserInformation","JiaoWu");
                }
            }
            return View();
        }
    }
}