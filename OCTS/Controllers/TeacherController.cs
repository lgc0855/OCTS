using OCTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace OCTS.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        DBHelper dbHelper = DBHelper.getMyHelper();
        public ActionResult CourseInfo()
        {

            //1.获取cookie和dbcontext
            HttpCookie accountCookie = Request.Cookies["Account"];
            List<TeacherCourse> TC = dbHelper.getTeacherCourses();

            List<Course> C = dbHelper.getCourses();
            //待保存的课程列表
            List<Course> temp = new List<Course>();
            string s = accountCookie["userName"];

            foreach (TeacherCourse tc in TC)
            {
                if (tc.userId == s)
                {
                    foreach (Course c in C)
                    {
                        if (c.courseId == tc.courseId)
                        {
                            temp.Add(c);
                        }
                    }
                }
            }

            return View(temp);
        }
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
            ViewData["courseid"] = id;
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

        public ActionResult EnterCourse(string id)
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
            HttpCookie hc = new HttpCookie("course");
            hc["courseid"] = id;
            hc["coursename"] =
                course.courseName;
            hc.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(hc);
            return View(course);
        }

        public ActionResult TaskSubmitInfo()
        {
            DBHelper dbHelper = DBHelper.getMyHelper();
            HttpCookie hc = Request.Cookies["course"];
            string courseId = null;
            if (hc != null)
                courseId = hc["courseid"];
            if (courseId == null)
                RedirectToAction("CourseInfo");
            List<TaskSubmit> ts = dbHelper.getTaskSubmits();
            List<TaskSubmit> temp = new List<TaskSubmit>();
            foreach (TaskSubmit t in ts)
            {
                if (t.courseId == courseId)
                    temp.Add(t);
            }
            return View(temp);
        }
        public ActionResult UserInformation()
        {
            DBHelper dbHelper = DBHelper.getMyHelper();
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
        public ActionResult Dafen(string gid, string tid, DateTime st, string cid)
        {
            if (tid == null || gid == null || cid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBHelper dbHelper = DBHelper.getMyHelper();
            List<TaskSubmit> ts = dbHelper.getTaskSubmits();
            TaskSubmitDBContext cdb = new TaskSubmitDBContext();
            TaskSubmit ts1 = new TaskSubmit();
            foreach (TaskSubmit ts2 in ts)
            {
                if (ts2.groupId == gid && ts2.taskId == tid && ts2.submitTime.CompareTo(st) == 0 && ts2.courseId == cid)
                    ts1 = ts2;
            }
            if (ts1 == null)
            {
                return HttpNotFound();
            }
            return View(ts1);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dafen([Bind(Include = "groupId,taskId,submitTime,submitterId,submitTimes,taskGrade,taskComment,courseId,taskUrl")] TaskSubmit tsb)
        {
            if (ModelState.IsValid)
            {
                TaskSubmitDBContext cdb = new TaskSubmitDBContext();
                cdb.Entry(tsb).State = EntityState.Modified;
                cdb.SaveChanges();
                return RedirectToAction("TaskSubmitInfo");
            }
            return View(tsb);
        }

        public FileResult Download(string url)
        {
            string[] name = url.Split('/');
            string Name = name[name.Length - 1];
            string contentType = "application/octet-stream";
            return File(url, contentType, Name);
        }

        public ActionResult UploadOutline(string courseID)
        {
            DBHelper dbHelper = DBHelper.getMyHelper();
            // string attachFile = ConfigurationSettings.AppSettings["attachFile"].Trim();
            string showFileName = Request["UploadName"];
            System.Web.HttpPostedFileBase file = Request.Files["UploadFile"];
            //存入文件  
            if (file.ContentLength > 0)
            {
                //先查看附件目录是否存在，不存在就创建，否则会报错 未找到路径。  
                //   if (!System.IO.File.Exists(attachFile))
                // {
                //这个是根据路径新建一个目录  
                //   System.IO.Directory.CreateDirectory(attachFile);
                //这个是根据路径新建一个文件,如果没有就会创建文件， 否则就会报错：对路径“...”的访问被拒绝。   
                //System.IO.File.Create(attachFile);  
                //}
                //这个是上传到当前项目的目录下，和Views文件夹同级  
                file.SaveAs(Server.MapPath("~/") + System.IO.Path.GetFileName(file.FileName));
                //这个是上传到指定的目录下，必须指定具体的文件，不然会报错：对路径“...”的访问被拒绝。   
                // file.SaveAs(attachFile + "\\" + System.IO.Path.GetFileName(showFileName));
            }
            //ViewBag.Result = System.IO.Path.GetFileName(file.FileName) + " 上传成功！";
            dbHelper.addOutline(courseID, Server.MapPath("~/") + System.IO.Path.GetFileName(file.FileName));
            return RedirectToAction("EditCourseInfo", "Teacher", new { id = courseID });
        }
        public ActionResult UploadRequire(string courseID)
        {
            DBHelper dbHelper = DBHelper.getMyHelper();
            //string attachFile = ConfigurationSettings.AppSettings["attachFile"].Trim();
            string showFileName = Request["UploadName"];
            System.Web.HttpPostedFileBase file = Request.Files["UploadFile"];
            //存入文件  
            if (file.ContentLength > 0)
            {
                //先查看附件目录是否存在，不存在就创建，否则会报错 未找到路径。  
                //  if (!System.IO.File.Exists(attachFile))
                //{
                //这个是根据路径新建一个目录  
                //  System.IO.Directory.CreateDirectory(attachFile);
                //这个是根据路径新建一个文件,如果没有就会创建文件， 否则就会报错：对路径“...”的访问被拒绝。   
                //System.IO.File.Create(attachFile);  
                //}
                //这个是上传到当前项目的目录下，和Views文件夹同级  
                file.SaveAs(Server.MapPath("~/") + System.IO.Path.GetFileName(file.FileName));
                //这个是上传到指定的目录下，必须指定具体的文件，不然会报错：对路径“...”的访问被拒绝。   
                //file.SaveAs(attachFile + "\\" + System.IO.Path.GetFileName(showFileName));
            }
            //ViewBag.Result = System.IO.Path.GetFileName(file.FileName) + " 上传成功！";
            dbHelper.addRequire(courseID, Server.MapPath("~/") + System.IO.Path.GetFileName(file.FileName));
            return RedirectToAction("EditCourseInfo", "Teacher", new { id = courseID });
        }


        /*
        public ActionResult Upload(string courseID)
        {
            string attachFile = ConfigurationSettings.AppSettings["attachFile"].Trim();
            string showFileName = Request["UploadName"];
            System.Web.HttpPostedFileBase file = Request.Files["UploadFile"];
            //存入文件  
            if (file.ContentLength > 0)
            {
                //先查看附件目录是否存在，不存在就创建，否则会报错 未找到路径。  
                if (!System.IO.File.Exists(attachFile))
                {
                    //这个是根据路径新建一个目录  
                    System.IO.Directory.CreateDirectory(attachFile);
                    //这个是根据路径新建一个文件,如果没有就会创建文件， 否则就会报错：对路径“...”的访问被拒绝。   
                    //System.IO.File.Create(attachFile);  
                }
                //这个是上传到当前项目的目录下，和Views文件夹同级  
                file.SaveAs(Server.MapPath("~/") + System.IO.Path.GetFileName(file.FileName));
                //这个是上传到指定的目录下，必须指定具体的文件，不然会报错：对路径“...”的访问被拒绝。   
                file.SaveAs(attachFile + "\\" + System.IO.Path.GetFileName(showFileName));
            }
            ViewBag.Result = System.IO.Path.GetFileName(file.FileName) + " 上传成功！";

            return RedirectToAction("EditCourseInfo", "Teacher", new { id=courseID});
        }  */
        public ActionResult SourceInfo()
        {
            DBHelper dbHelper = DBHelper.getMyHelper();
            HttpCookie hc = Request.Cookies["course"];
            string courseId = null;
            if (hc != null)
                courseId = hc["courseid"];
            if (courseId == null)
                RedirectToAction("CourseInfo");
            List<Source> source = dbHelper.getSources();
            List<Source> temp = new List<Source>();
            foreach (Source s in source)
            {
                if (s.courseId == courseId)
                    temp.Add(s);
            }
            return View(temp);
        }

        public ActionResult Index()
        {

            return RedirectToAction("CourseInfo");
        }

        //显示作业清单信息
        public ActionResult TaskListInfo()
        {
            //1.获取Cookie，从中提取课程id
            HttpCookie cookie = Request.Cookies["course"];
            string courseId = null;
            string courseName = null;
            if (cookie != null)
            {
                courseId = cookie["courseid"];
                courseName = cookie["coursename"];
            }
            if (courseId == null || courseName == null)
            {
                return RedirectToAction("CourseInfo");
            }

            //2.根据课程id，获取全部作业清单tmplist
            TaskInformationDBContext db = new TaskInformationDBContext();
            List<TaskInformation> tmplist = db.taskInformations.ToList();

            //3.根据课程id筛选作业信息保存到作业清单list中
            List<TaskInformation> list = new List<TaskInformation>();
            foreach (TaskInformation t in tmplist)
            {
                if (t.courseId == courseId)
                {
                    list.Add(t);
                }
            }

            ViewData["courseName"] = courseName;
            return View(list);
        }

        //创建作业信息
        public ActionResult CreateTaskInfo()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTaskInfo([Bind(Include = "taskName,taskStartTime,taskEndTime,taskMaxSubmitTime,taskRequire")]TaskInformation task)
        {
            DBHelper dbHelper = DBHelper.getMyHelper();
            if (ModelState.IsValid)
            {
                //1.获取Cookie，从中提取课程id
                HttpCookie cookie = Request.Cookies["course"];
                string courseId = null;
                if (cookie != null)
                {
                    courseId = cookie["courseid"];
                }
                if (courseId == null)
                {
                    return RedirectToAction("CourseInfo");
                }
                //2.赋值给task
                task.courseId = courseId;

                //3.获取cookie，提取用户id，获得用户姓名
                cookie = Request.Cookies["Account"];
                string userId = null;
                if (cookie != null)
                {
                    userId = cookie["userName"];
                }
                if (userId == null)
                {
                    return RedirectToAction("Index", "SecurityController");
                }
                List<User> users = dbHelper.getUsers();
                foreach (User u in users)
                {
                    if (u.userId == userId)
                    {
                        task.taskSubmitter = u.userName;
                        break;
                    }
                }
                TaskInformationDBContext db = new TaskInformationDBContext();
                db.taskInformations.Add(task);
                db.SaveChanges();
                db = null;
            }
            return RedirectToAction("TaskListInfo");
        }


        public ActionResult EditTaskInfo(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int ID = Convert.ToInt32(id);
            TaskInformation task = new TaskInformationDBContext().taskInformations.Find(ID);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }


        //编辑作业信息
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTaskInfo([Bind(Include = "taskId,taskName,taskStartTime,taskEndTime,taskMaxSubmitTime,taskPercent,taskRequire,taskSubmitter,courseId")] TaskInformation task)
        {
            if (ModelState.IsValid)
            {
                TaskInformationDBContext db = new TaskInformationDBContext();
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("TaskListInfo");
        }

        public ActionResult DeleteTaskInfo(string id)
        {
            TaskInformationDBContext db = new TaskInformationDBContext();
            int ID = Convert.ToInt32(id);
            TaskInformation taskinformation = db.taskInformations.Find(ID);
            db.taskInformations.Remove(taskinformation);
            db.SaveChanges();
            return RedirectToAction("TaskListInfo");
        }


        public ActionResult TaskInfoDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskInformationDBContext db = new TaskInformationDBContext();
            int ID = Convert.ToInt32(id);
            TaskInformation taskinformation = db.taskInformations.Find(ID);
            if (taskinformation == null)
            {
                return HttpNotFound();
            }

            return View(taskinformation);
        }


        public ActionResult ResourceManage()
        {
            return View();
        }


        public string uploadFiles(HttpPostedFileBase file)
        {
            try
            {
                HttpCookie cookie = Request.Cookies["Account"];
                User u = dbHelper.findUser(cookie["userName"]);
                var savePath = this.Server.MapPath("/TeacherResource/" + u.userName + "/" + file.FileName);
                file.SaveAs(savePath);
            }catch(Exception e)
            {
                return "{\"error\":\"在服务器端发生错误请联系管理员\"}";
            }
            return "{\"success\":\"\"}";
        }



        public string getResources( string folderPath)
        {
            string[] resultFile;
            string[] resultDirectories;
            HttpCookie cookie = Request.Cookies["Account"];
            User u = dbHelper.findUser(cookie["userName"]);
            if(folderPath == "")
            {
                folderPath ="/"+ u.userName;
            }
          /*  else
            {
                folderPath = folderPath.Replace(",", "/");
            }
            */
            var severPath = this.Server.MapPath("/TeacherResource" + folderPath + "/");
            Directory.CreateDirectory(severPath);
            try
            {
                resultFile = Directory.GetFiles(severPath);
                resultDirectories = Directory.GetDirectories(severPath);
            }
            catch (Exception e)
            {
                return "error";
            }
            TeacherResource resource = new TeacherResource();
            resource.files = resultFile;
            resource.directories = resultDirectories;
            return JsonConvert.SerializeObject(resource);
        }


        private class TeacherResource
        {
            public string[] files { get; set; }
            public string[] directories { get; set; }

        }
    }
}
