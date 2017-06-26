using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OCTS.Models ;
using System.Web.Security;

namespace OCTS.Controllers
{
    public class SecurityController : Controller
    {
        private DBHelper dbHelper = DBHelper.getMyHelper();
        // GET: Security
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            // string[] cookies = Request.Cookies.AllKeys;
            if (accountCookie != null)
            {
                List<User> list = dbHelper.getUsers();
                string userName = accountCookie["userName"];
                string password = accountCookie["password"];
                string userType = accountCookie["userType"];
                foreach (User user in list)
                {
                    if (user.userId == userName)
                    {
                        if (password == user.userPassword)
                        {
                            if (userType == "0")
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else if (userType == "1")
                            {
                                return RedirectToAction("Index", "JiaoWu");
                            }
                            else if (userType == "2")
                            {
                                return RedirectToAction("Index", "Teacher");
                            }
                            else if (userType == "3")
                            {
                                return RedirectToAction("Index", "Student");
                            }
                        }
                    }
                }
            }
            return View();
        }


        [HttpPost]
        public ActionResult Login( string userName , string password , string userType)
        {
            List<User> list = dbHelper.getUsers();
            password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            foreach (User user in list)
            {
                if (user.userId == userName)
                {
                    if (password == user.userPassword)
                    {
                        if( user.userType.ToString() == userType)
                        {
                            HttpCookie accountCookie = new HttpCookie("Account");
                            accountCookie["userName"] = userName;
                            accountCookie["password"] = password;
                            accountCookie["userType"] = userType;
                            accountCookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(accountCookie);
                            if (userType == "admin")
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else if(userType == "jiaowu")
                            {
                                return RedirectToAction("Index", "JiaoWu");
                            }else if(userType == "teacher")
                            {
                                return RedirectToAction("Index", "Teacher");
                            }else if(userType == "student")
                            {
                                return RedirectToAction("Index", "Student");
                            }
                        }
                        
                    }
                }
            }
            ViewData["ErrorMessage"] = "用户名密码不正确";
            return View();
        }

        public ActionResult ForgetMessage()
        {
            return View();
        }
    }
}