using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OCTS.Models;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Microsoft.Office.Interop.Excel;
using System.Web.Security;
using System.Net;
using System.Data.Entity;

namespace OCTS.Controllers
{
    public class JiaoWuController : Controller
    {

        private DBHelper dbHelper = new DBHelper();
        // GET: JiaoWu
        public ActionResult Index()
        {
            int week;
            string nowWeeks = "";
            DateTime[] term = dbHelper.getNewTermInformation();
            string startTime;
            string endTime;
            string year;
            TermInfo t = new TermInfo();
            if(term != null)
            {
                startTime = term[0].ToString("yyyy MM dd");
                endTime = term[1].ToString("yyyy MM dd");
                year = term[0].ToString("yyyy");
                DateTime now = System.DateTime.Now;
                TimeSpan duration = now - term[0];
                int days  = duration.Days;
                week = days / 7;
                t.startTime = startTime;
                t.endTime = endTime;
                t.weeks = week.ToString();
                t.year = year;
                ViewData["TermInformation"] = t;
            }
            

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


        public string batchAddStudents(HttpPostedFileBase file)
        {
            var severPath = this.Server.MapPath("/ExcelFiles/");
            Directory.CreateDirectory(severPath);
            var savePath = Path.Combine(severPath, file.FileName);
            string result = "{}";
            try
            {
                if (string.Empty.Equals(file.FileName) || ".xlsx" != Path.GetExtension(file.FileName))
                {
                    return "error";
                }

                file.SaveAs(savePath);
                //启动Excel应用程序
                Microsoft.Office.Interop.Excel.Application xls = new Microsoft.Office.Interop.Excel.Application();
                //打开filename表
                _Workbook book = xls.Workbooks.Open(savePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                _Worksheet sheet;//定义sheet变量
                xls.Visible = false;//设置Excel后台运行
                xls.DisplayAlerts = false;//设置不显示确认修改提示

                try
                {
                    sheet = (_Worksheet)book.Worksheets.get_Item(1);//获得第index个sheet，准备读取
                }
                catch (Exception ex)//不存在就退出
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                Console.WriteLine(sheet.Name);
                int row = sheet.UsedRange.Rows.Count;//获取不为空的行数
                int col = sheet.UsedRange.Columns.Count;//获取不为空的列数
                // Array value = (Array)sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[row, col]).Cells.Value2;//获得区域数据赋值给Array数组，方便读取\
                string tempId;
                string tempName;
                int idcol = -1;
                int idrow = -1;
                int namecol = -1;
                for (var i = 1; i <= row; i++)
                {
                    for (var j = 1; j <= col; j++)
                    {
                        tempId = ((Range)sheet.Cells[i, j]).Text;
                        if (tempId.Equals("学号"))
                        {
                            idcol = j;
                            idrow = i;
                        }
                        if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                    }

                    if (idcol >= 0 && idrow >= 0)
                    {
                        break;
                    }
                }

                UserDBContext userdb = new UserDBContext();

                for (var i = idrow + 1; i <= row; i++)
                {
                    tempId = ((Range)sheet.Cells[i, idcol]).Text;
                    tempName = ((Range)sheet.Cells[i, namecol]).Text;
                    if (isInt(tempId) && tempName != "")
                    {
                        User user = new Models.User();
                        user.userId = tempId;
                        user.userName = tempName;
                        user.setUserType("student");
                        user.userPassword = "8CB2237D0679CA88DB6464EAC60DA96345513964";
                        userdb.users.Add(user);
                    }
                }

                userdb.SaveChanges();


                book.Save();//保存
                book.Close(false, Missing.Value, Missing.Value);//关闭打开的表
                xls.Quit();//Excel程序退出
                //sheet,book,xls设置为null，防止内存泄露
                sheet = null;
                book = null;
                xls = null;
                GC.Collect();//系统回收资源
                System.IO.File.Delete(savePath);
                result = "{}";
            }
            catch (Exception e)
            {
                result = "{\"error\":\"在服务器端发生错误请联系管理员\"}";
            }
            finally
            {
                System.IO.File.Delete(savePath);
            }
            return result;
        }
        public string Test()
        {
            return "";
        }

        public string batchAddTeachers(HttpPostedFileBase file)
        {
            var severPath = this.Server.MapPath("/ExcelFiles/");
            Directory.CreateDirectory(severPath);
            var savePath = Path.Combine(severPath, file.FileName);
            string result = "{}";
            try
            {
                if (string.Empty.Equals(file.FileName) || ".xlsx" != Path.GetExtension(file.FileName))
                {
                    return "error";
                }

                file.SaveAs(savePath);
                //启动Excel应用程序
                Microsoft.Office.Interop.Excel.Application xls = new Microsoft.Office.Interop.Excel.Application();
                //打开filename表
                _Workbook book = xls.Workbooks.Open(savePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                _Worksheet sheet;//定义sheet变量
                xls.Visible = false;//设置Excel后台运行
                xls.DisplayAlerts = false;//设置不显示确认修改提示

                try
                {
                    sheet = (_Worksheet)book.Worksheets.get_Item(1);//获得第index个sheet，准备读取
                }
                catch (Exception ex)//不存在就退出
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
                Console.WriteLine(sheet.Name);
                int row = sheet.UsedRange.Rows.Count;//获取不为空的行数
                int col = sheet.UsedRange.Columns.Count;//获取不为空的列数
                // Array value = (Array)sheet.get_Range(sheet.Cells[1, 1], sheet.Cells[row, col]).Cells.Value2;//获得区域数据赋值给Array数组，方便读取\
                string tempId;
                string tempName;
                int idcol = -1;
                int idrow = -1;
                int namecol = -1;
                for (var i = 1; i <= row; i++)
                {
                    for (var j = 1; j <= col; j++)
                    {
                        tempId = ((Range)sheet.Cells[i, j]).Text;
                        if (tempId.Equals("工号"))
                        {
                            idcol = j;
                            idrow = i;
                        }
                        if (tempId.Equals("姓名"))
                        {
                            namecol = j;
                        }
                    }

                    if (idcol >= 0 && idrow >= 0)
                    {
                        break;
                    }
                }

                UserDBContext userdb = new UserDBContext();

                for (var i = idrow + 1; i <= row; i++)
                {
                    tempId = ((Range)sheet.Cells[i, idcol]).Text;
                    tempName = ((Range)sheet.Cells[i, namecol]).Text;
                    if (isInt(tempId) && tempName != "")
                    {
                        User user = new Models.User();
                        user.userId = tempId;
                        user.userName = tempName;
                        user.setUserType("teacher");
                        user.userPassword = "8CB2237D0679CA88DB6464EAC60DA96345513964";
                        userdb.users.Add(user);
                    }
                }

                userdb.SaveChanges();


                book.Save();//保存
                book.Close(false, Missing.Value, Missing.Value);//关闭打开的表
                xls.Quit();//Excel程序退出
                //sheet,book,xls设置为null，防止内存泄露
                sheet = null;
                book = null;
                xls = null;
                GC.Collect();//系统回收资源
                System.IO.File.Delete(savePath);
                result = "{\"success\":\"成功导入\"}";
            }
            catch (Exception e)
            {
                result = "{\"error\":\"在服务器端发生错误请联系管理员\"}";
            }
            finally
            {
                System.IO.File.Delete(savePath);
            }
            return result;
        }

        private bool isInt(string s)
        {
            int i = 0 ;
            try
            {
                i = int.Parse(s);
            } catch (Exception e)
            {
                return false;
            }

            return true;
        }


        public class TermInfo
        {
            public string year { get; set; }
            public string startTime { get; set; }
            public  string endTime { get; set; }
            public string weeks { get; set; }
        }
        public ActionResult UserInformation()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            List<User> list = dbHelper.getUsers();
            string userName = accountCookie["userName"];
            foreach (User user in list)
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
        public ActionResult Update(string name, string email, string phone, string password)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            List<User> list = dbHelper.getUsers();
            string userName = accountCookie["userName"];
            foreach (User user in list)
            {
                if (userName == user.userId)
                {
                    dbHelper.setUsers(userName, name, email, phone, password);
                    return RedirectToAction("UserInformation", "JiaoWu");
                }
            }
            return View();
        }
        public ActionResult CourseInfo()
        {
            //获取course的list列表
            List<Course> courseList = dbHelper.getCourses();
            //把courseList通过model发给视图
            return View(courseList);
        }

        public ActionResult CreateCourseInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourseInfo(string courseName, string courseStartTime, string courseEndTime, string courseTeacherName, string courseCredit, string coursePlace, string courseTime)
        {
            dbHelper.addCourse(courseName, courseStartTime, courseEndTime, coursePlace, courseTeacherName, courseCredit, courseTime);
            return RedirectToAction("CourseInfo");
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "courseName,courseStartTime,courseEndTime,courseTeacherName,courseCredit,coursePlacep,courseTime")] Course course)
        {
            if (ModelState.IsValid)
            {
                string coursesId = System.DateTime.Now.ToLongDateString();
                course.courseId = coursesId;
                course.courseCredit = 0;
                course.maxNumPerGroup = 0;
                course.minNumPerGroup = 0;
                course.isEnd = false;
                db.courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        */
        public ActionResult EditCourseInfo(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDBContext cdb = new CourseDBContext();
            Course course = cdb.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourseInfo([Bind(Include = "courseId,courseName,courseStartTime,courseEndTime,courseRequireURL,courseOutlineURL,courseTeacherName,courseCredit,coursePlace,maxNumPerGroup,minNumPerGroup,courseTime,isEnd")] Course course)
        {
            if (ModelState.IsValid)
            {
                CourseDBContext cdb = new CourseDBContext();
                cdb.Entry(course).State = EntityState.Modified;
                cdb.SaveChanges();
                return RedirectToAction("CourseInfo");
            }
            return View(course);
        }

        public ActionResult CourseDetails(string id)
        {
            CourseDBContext db = new CourseDBContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        public ActionResult DeleteCourseInfo(string id)
        {
            CourseDBContext db = new CourseDBContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        // POST: /JCI/Delete/5
        [HttpPost, ActionName("DeleteCourseInfo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CourseDBContext db = new CourseDBContext();
            Course course = db.courses.Find(id);
            db.courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("CourseInfo");
        }
        protected override void Dispose(bool disposing)
        {
            CourseDBContext db = new CourseDBContext();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult StudentInfo()
        {
            UserDBContext db = new UserDBContext();
            List<User> list = db.users.ToList();
            List<User> temp = new List<User>();
            foreach (User u in list)
            {
                if (u.userType == Models.User.userTypeNum.student)
                    temp.Add(u);
            }
            return View(temp);
        }

        public ActionResult EditStudentInfo(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDBContext cdb = new UserDBContext();
            User user = cdb.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudentInfo([Bind(Include = "userId,userName,userPassword,userType,userEmail,userPhone,userSex")] User user)
        {
            if (ModelState.IsValid)
            {
                UserDBContext cdb = new UserDBContext();
                user.userPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.userPassword, "SHA1");
                cdb.Entry(user).State = EntityState.Modified;
                cdb.SaveChanges();
                return RedirectToAction("StudentInfo");
            }
            return View(user);
        }

        public ActionResult StudentInfoDetails(string id)
        {
            UserDBContext db = new UserDBContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult DeleteStudentInfo(string id)
        {
            UserDBContext db = new UserDBContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // POST: /JCI/Delete/5
        [HttpPost, ActionName("DeleteStudentInfo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed1(string id)
        {
            UserDBContext db = new UserDBContext();
            User user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("StudentInfo");
        }

        public ActionResult TeacherInfo()
        {
            UserDBContext db = new UserDBContext();
            List<User> list = db.users.ToList();
            List<User> temp = new List<User>();
            foreach (User u in list)
            {
                if (u.userType == Models.User.userTypeNum.teacher)
                    temp.Add(u);
            }
            return View(temp);
        }

        public ActionResult EditTeacherInfo(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDBContext cdb = new UserDBContext();
            User user = cdb.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTeacherInfo([Bind(Include = "userId,userName,userPassword,userType,userEmail,userPhone,userSex")] User user)
        {
            if (ModelState.IsValid)
            {
                UserDBContext cdb = new UserDBContext();
                user.userPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.userPassword, "SHA1");
                cdb.Entry(user).State = EntityState.Modified;
                cdb.SaveChanges();
                return RedirectToAction("TeacherInfo");
            }
            return View(user);
        }

        public ActionResult TeacherInfoDetails(string id)
        {
            UserDBContext db = new UserDBContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult DeleteTeacherInfo(string id)
        {
            UserDBContext db = new UserDBContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        // POST: /JCI/Delete/5
        [HttpPost, ActionName("DeleteTeacherInfo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed2(string id)
        {
            UserDBContext db = new UserDBContext();
            User user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("TeacherInfo");
        }
    }
}