using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OCTS.Models;
using System.IO;
using System.Text;
using System.Configuration;

namespace OCTS.Controllers
{
    public class StudentController : Controller
    {
        private DBHelper dbHelper = DBHelper.getMyHelper();
        // GET: Student
        public ActionResult Index()
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            List<UserCourse> uclist = dbHelper.getUserCourses();
            List<Course> clist = dbHelper.getCourses();
            string userName = accountCookie["userName"];
            int num = 0;
            foreach (UserCourse usercourse in uclist)
            {
                if (usercourse.userId == userName)
                {                  
                    foreach (Course course in clist)
                    {
                        if(course.courseId==usercourse.courseId)
                        {
                            num++;
                            ViewData["courseid" + num] = course.courseId;
                            ViewData["coursename"+num] = course.courseName;
                            ViewData["teacher"+num] = course.courseTeacherName;
                            ViewData["xuefen"+num] = course.courseCredit;
                        }
                    }
                    ViewData["max"] = num;
                }                       
            }
            return View();
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
        public ActionResult ucourseinfo(string id)
        {
            List<Course> clist = dbHelper.getCourses();
            foreach(Course course in clist)
            {
                if (id == course.courseId)
                {
                    ViewData["courseid"] = course.courseId;
                    ViewData["coursename"] = course.courseName;
                    ViewData["coursestart"] = course.courseStartTime;
                    ViewData["courseend"] = course.courseEndTime;
                    ViewData["courserequrl"] = course.courseRequireURL;
                    ViewData["courseouturl"] = course.courseOutlineURL;
                    ViewData["courseteacher"] = course.courseTeacherName;
                    ViewData["coursecredit"] = course.courseCredit;
                    ViewData["coursemaxnum"] = course.maxNumPerGroup;
                    ViewData["courseminnum"] = course.minNumPerGroup;
                    ViewData["courseplace"] = course.coursePlace;
                    ViewData["coursetime"] = course.courseTime;
                    ViewData["courseisend"] = course.isEnd;
                }
            }
            return View();
        }
        public ActionResult CourseOutLine(string courseID)
        {
            string outlineUrl = null;
            List<Course> clist = dbHelper.getCourses();
            foreach (Course course in clist)
            {
                if (courseID == course.courseId)
                {
                   outlineUrl = course.courseOutlineURL;
                }
            } //通过模板页传递的课程id查询课程要求url
            if (outlineUrl != null)
            {
                ViewData["url"] = outlineUrl;
                //string url = Server.MapPath(outlineUrl);
                string url = outlineUrl;
                StreamReader sr = new StreamReader(url, Encoding.Default);
                StringBuilder sb = new StringBuilder();
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
                string result = sb.ToString();
                ViewData["txt"] = result;
            }
            ViewData["courseid"] = courseID;                    //传递课程id，用于侧栏菜单跳转
            return View();
        }
        public ActionResult CourseRequest(string courseID)
        {
            string requestUrl = null;
            List<Course> clist = dbHelper.getCourses();
            foreach (Course course in clist)
            {
                if (courseID == course.courseId)
                {
                    requestUrl = course.courseRequireURL;
                }
            }                                                    //通过模板页传递的课程id查询课程要求url
            if (requestUrl != null)
            {
                ViewData["requrl"] = requestUrl;
                //string url = Server.MapPath(requestUrl);
                string url = requestUrl;
                StreamReader sr = new StreamReader(url, Encoding.Default);
                StringBuilder sb = new StringBuilder();
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
                string result = sb.ToString();
                ViewData["reqtxt"] = result;
            }
            ViewData["courseid"] = courseID;                    //传递课程id，用于侧栏菜单跳转
            return View();
        }
        public ActionResult CourseSource(string courseID)
        {
            List<Source> slist = dbHelper.getSources();
            int sourceNum = 0;
            foreach (Source source in slist)
            {
                if (source.courseId == courseID)
                {
                    sourceNum++;
                    ViewData["sourcename"+sourceNum] = source.sourceName;
                    ViewData["sourcesubmitter"+sourceNum] = source.sourceSubmitter;
                    ViewData["sourcetime"+sourceNum] = source.sourceTime;
                    ViewData["sourceurl" + sourceNum] = source.sourceURL;

                }
            }
            ViewData["sourcenum"] = sourceNum;
            ViewData["courseid"] = courseID;
            return View();
        }
        public ActionResult StudentHandin(string courseID)
        {
            List<TaskInformation> tlist = dbHelper.getTaskInformations();
            int taskNum = 0;
            foreach (TaskInformation taskinfo in tlist)
            {
                if (taskinfo.courseId == courseID)
                {
                    taskNum++;
                    ViewData["taskid"+taskNum] = taskinfo.taskId;
                    ViewData["taskstart"+taskNum] = taskinfo.taskStartTime;
                    ViewData["taskend"+taskNum] = taskinfo.taskEndTime;
                    ViewData["taskreq" + taskNum] = taskinfo.taskRequire;
                    ViewData["taskmaxtimes"+taskNum] = taskinfo.taskMaxSubmitTime;
                    ViewData["taskpercent" + taskNum] = taskinfo.taskPercent;
                    ViewData["taskpeople" + taskNum] = taskinfo.taskSubmitter;
                }
            }
            ViewData["tasknum"] = taskNum;
            ViewData["courseid"] = courseID;
            return View();
        }
        public ActionResult TaskInfo(string TaskReq,string courseId,string TaskId,string TaskMaxTimes)
        {
            HttpCookie accountCookie = Request.Cookies["Account"];
            string userId = accountCookie["userName"];
            ViewData["userid"] = userId;
            ViewData["taskreq"] = TaskReq;
            ViewData["taskid"] = TaskId;
            ViewData["taskmaxtimes"] = TaskMaxTimes;
            List<TaskSubmit> TSlist = dbHelper.getTaskSubmits();
            List<User> ulist = dbHelper.getUsers();
            List<Group> glist = dbHelper.getGroups();
            int subNum = 0;
            foreach (TaskSubmit subinfo in TSlist)
            {
                if (subinfo.courseId == courseId&&TaskId==subinfo.taskId&&userId==subinfo.userId)
                {
                    subNum++;
                    ViewData["taskid" + subNum] = subinfo.taskId;
                    ViewData["subtime" + subNum] = subinfo.submitTime;
                    ViewData["groupid" + subNum] = subinfo.groupId;
                    ViewData["comment" + subNum] = subinfo.taskComment;
                    ViewData["grade" + subNum] = subinfo.taskGrade;
                    foreach(User user in ulist)
                    {
                        if (user.userId == subinfo.submitterId)
                        {
                            ViewData["username"+subNum] = user.userName;
                        }
                    }
                    foreach(Group group in glist)
                    {
                        if (group.groupId == subinfo.groupId)
                        {
                            ViewData["leaderid"] = group.groupLeaderId;
                            foreach(User user1 in ulist)
                            {
                                if (user1.userId == group.groupLeaderId)
                                {
                                    ViewData["leader"+subNum] = user1.userName;
                                }
                            }
                            
                        }
                    }
                }
            }
            ViewData["subnum"] = subNum;
            ViewData["courseid"] = courseId;
            return View();
        }
        public ActionResult Upload(string taskreq, string courseid, string taskid, string taskmaxtimes)
        {

            /// <summary>  
            /// 上传操作  
            /// </summary>  
            /// <returns></returns>
            //string attachFile = ConfigurationSettings.AppSettings["attachFile"].Trim();
            string showFileName = Request["UploadName"];
            System.Web.HttpPostedFileBase file = Request.Files["UploadFile"];
            //存入文件  
            if (file.ContentLength > 0)
            {
                //先查看附件目录是否存在，不存在就创建，否则会报错 未找到路径。  
                //if (!System.IO.File.Exists(attachFile))
                //{
                    //这个是根据路径新建一个目录  
                 //   System.IO.Directory.CreateDirectory(attachFile);  
                //这个是根据路径新建一个文件,如果没有就会创建文件， 否则就会报错：对路径“...”的访问被拒绝。   
                //System.IO .File.Create(attachFile);
                //}
                //这个是上传到当前项目的目录下，和Views文件夹同级  
                file.SaveAs(Server.MapPath("~/homework/") + System.IO.Path.GetFileName(file.FileName));
                //这个是上传到指定的目录下，必须指定具体的文件，不然会报错：对路径“...”的访问被拒绝。   
                //file.SaveAs(attachFile + "\\" + System.IO.Path.GetFileName(showFileName));
            }
            //ViewBag.Result = System.IO.Path.GetFileName(file.FileName) + " 上传成功！";
            //List<TaskSubmit> tslist = dbHelper.getTaskSubmits();
            List<UserCourse> uclist = dbHelper.getUserCourses();
            HttpCookie accountCookie = Request.Cookies["Account"];
            string userId = accountCookie["userName"];
            string groupid="0";
            foreach(UserCourse usercourse in uclist)
            {
                if(usercourse.userId==userId&&usercourse.courseId==courseid)
                {
                    groupid = usercourse.groupId;
                }
            }
            DateTime submittime =System.DateTime.Now;
            string submittimes = "0";
            int taskgrade = 0;
            string taskcomment = "";
            string submitterid = userId;
            string taskurl = Server.MapPath("~/homework/") + System.IO.Path.GetFileName(file.FileName);
            dbHelper.addTaskSubmit(groupid, taskid, submittime, userId, submittimes, taskgrade, taskcomment, submitterid, courseid, taskurl);
            
            return RedirectToAction("TaskInfo", "Student", new { TaskReq=taskreq,courseId=courseid,TaskId=taskid,TaskMaxTimes=taskmaxtimes});
        }
        public FileResult Download(string outUrl)
        {
            string[] strol = outUrl.Split(new char[] { '/' });
            string name = strol[strol.Length - 1];
            string contentType = "application/octet-stream";
            return File(outUrl, contentType, name);
        }
    }
}