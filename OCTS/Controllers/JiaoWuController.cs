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

        private DBHelper dbHelper = new DBHelper();
        // GET: JiaoWu
        public ActionResult Index()
        {
            return View();
        }

        public string AddSemester(string startTime , string endTime)
        {
            char c = ' ';
            string[] temp = startTime.Split(c);
            int year = Convert.ToInt32(temp[2]);
            int month = Convert.ToInt32(temp[1]);
            int day = Convert.ToInt32(temp[0]);
            DateTime sTime = new DateTime(year,month,day);
            temp = endTime.Split(c);
            year = Convert.ToInt32(temp[2]);
            month = Convert.ToInt32(temp[1]);
            day = Convert.ToInt32(temp[0]);
            DateTime eTime = new DateTime(year,month,day);
            TimeSpan duration = eTime - sTime;
            int i = 0;
            TimeSpan basic = new TimeSpan(24, 0, 0);
            int result = duration.CompareTo(basic);
            if(result <=  0)
            {
                return "设置学期天数必须大于一天！";
            }
            day = duration.Days;
            string s = dbHelper.addSemester(sTime, eTime, day / 7 + 1);
            if(s.StartsWith("succeed"))
            return "成功设置学期！";

            return "设置学期失败！";
        }
    }
}