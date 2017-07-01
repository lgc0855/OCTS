using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.IO.Compression;


namespace OCTS.Models
{
    public class DBHelper
    {
        public static DBHelper myHelper = null;

        public static DBHelper getMyHelper()
        {
            myHelper = new DBHelper();
            return myHelper;
        }

        public List<User> getUsers()
        {
            UserDBContext userdb = new UserDBContext();
            List<User> users = userdb.users.ToList();
            userdb = null;
            return users;
        }
        public void setUsers(string userid, string username, string useremail, string userphone, string password)
        {
            UserDBContext userdb = new UserDBContext();
            List<User> users = userdb.users.ToList();
            foreach (User user in users)
            {
                if(user.userId==userid)
                {
                    user.userName = username;
                    user.userEmail = useremail;
                    user.userPhone = userphone;
                    user.userPassword = password;
                    userdb.SaveChanges(); 
                }
            }
        }
        public void setUsersPassword(string userid,string password)
        {
            UserDBContext userdb = new UserDBContext();
            List<User> users = userdb.users.ToList();
            foreach (User user in users)
            {
                if (user.userId == userid)
                {
                    user.userPassword = password;
                    userdb.SaveChanges();
                }
            }
        }
        public List<Course> getCourses()
        {
            CourseDBContext coursedb = new CourseDBContext();
            List<Course> courses = coursedb.courses.ToList();
            coursedb = null;
            return courses;
        }
        public List<TaskInformation> getTaskInformations()
        {
            TaskInformationDBContext taskInformationdb = new TaskInformationDBContext();
            List<TaskInformation> taskInformations = taskInformationdb.taskInformations.ToList();
            taskInformationdb = null;
            return taskInformations;
        }
        public List<TermInformation> getTermInformations()
        {
            TermInformationDBContext termInformationdb = new TermInformationDBContext();
            List<TermInformation> termInformations = termInformationdb.termInformations.ToList();
            termInformationdb = null;
            return termInformations;
        }
        public List<Source> getSources()
        {
            SourceDBContext sourcedb = new SourceDBContext();
            List<Source> sources = sourcedb.sources.ToList();
            sourcedb = null;
            return sources;
        }
        public List<UserCourse> getUserCourses()
        {
            UserCourseDBContext userCoursedb = new UserCourseDBContext();
            List<UserCourse> userCourses = userCoursedb.userCourses.ToList();
            userCoursedb = null;
            return userCourses;
        }
        public List<TeacherCourse> getTeacherCourses()
        {
            TeacherCourseDBContext teacherCoursedb = new TeacherCourseDBContext();
            List<TeacherCourse> teacherCourses = teacherCoursedb.teacherCourses.ToList();
            teacherCoursedb = null;
            return teacherCourses;
        }
        public List<Group> getGroups()
        {
            GroupDBContext groupdb = new GroupDBContext();
            List<Group> groups = groupdb.groups.ToList();
            groupdb = null;
            return groups;
        }

        //查找某课程的group
        public List<Group> getGroups(string courseId)
        {
            GroupDBContext gdb = new GroupDBContext();
            List<Group> gList = gdb.groups.ToList();
            List<Group> result = new List<Group>();
            foreach(Group g in gList)
            {
                if(g.courseId == courseId)
                {
                    result.Add(g);
                }
            }
            return result;
        }


        //查找某个小组的成员
        public List<User> getGroupMembers( string groupId)
        {
            UserCourseDBContext ucdb = new UserCourseDBContext();
            List<UserCourse> list = ucdb.userCourses.ToList();
            List<UserCourse> tempList = ucdb.userCourses.ToList();
            List<User> resultList = new List<User>();
            foreach(UserCourse u in list)
            {
                if(u.groupId == groupId)
                {
                    tempList.Add(u);
                }
            }
            foreach(UserCourse uc in tempList)
            {
                User u = findUser(uc.userId);
                if(u!=null)
                {
                    resultList.Add(u);
                }
            }
            ucdb = null;
            return resultList;


        }

        public List<TaskSubmit> getTaskSubmits()
        {
            TaskSubmitDBContext tasksubmitdb = new TaskSubmitDBContext();
            List<TaskSubmit> taskSubmits = tasksubmitdb.taskSubmits.ToList();
            tasksubmitdb = null;
            return taskSubmits;
        }
        //学期有关操作
        public string addSemester(DateTime termStartTime, DateTime termEndTime , int termWeeks)
        {
            string result = "";
            int num = 0;
            TermInformationDBContext tIdb = new TermInformationDBContext();
            TermInformation term = new TermInformation();
            try
            {
                term.termStartTime = termStartTime;
                term.termEndTime = termEndTime;
                term.termWeeks = termWeeks;
                tIdb.termInformations.Add(term);
                num = tIdb.SaveChanges();
                result = "succeed:写入" + num + "条信息";

            }catch(Exception e)
            {
                result = "error:写入" + num + "条信息"; 
            }
            return result;
        }

        //查找用户
        public User findUser(string userId)
        {
            UserDBContext userdb = new UserDBContext();
            List<User> users = userdb.users.ToList();
            foreach(User u in users)
            {
                if(u.userId == userId)
                {
                    return u;
                }
            }
            return null;
        }
        public void addTaskSubmit(string groupid, string taskid, DateTime submittime, string userId, string submittimes, int taskgrade, string taskcomment, string submitterid, string courseid, string taskurl)
        {

            //TermInformationDBContext tIdb = new TermInformationDBContext();
            //TermInformation term = new TermInformation();
            int num;
            TaskSubmitDBContext tsdb = new TaskSubmitDBContext();
            TaskSubmit ts = new TaskSubmit();
            try
            {
                ts.groupId = groupid;
                ts.taskId = taskid;
                ts.submitTime = submittime;
                ts.userId = userId;
                ts.submitTimes = submittimes;
                ts.taskGrade = taskgrade;
                ts.taskComment = taskcomment;
                ts.submitterId = submitterid;
                ts.courseId = courseid;
                ts.taskUrl = taskurl;
                tsdb.taskSubmits.Add(ts);
                num = tsdb.SaveChanges();
                // tIdb.termInformations.Add(term);
                //tIdb.SaveChanges();

            }
            catch (Exception e)
            {

            }
        }
        //获取最新学期信息
        public DateTime[] getNewTermInformation()
        {
            TermInformationDBContext termdb = new TermInformationDBContext();
            DateTime[] result = new DateTime[2];
            TermInformation[] terms = termdb.termInformations.ToArray();
            if(terms.Length > 0)
            {
                result[0] = terms[terms.Length - 1].termStartTime;
                result[1] = terms[terms.Length - 1].termEndTime;
                return result;
            }
            
            return null;

        }
        //添加课程信息
        public void addCourse(string name, string start, string end, string address, string teacher)
        {
            CourseDBContext db = new CourseDBContext();

            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            DateTime dt1 = Convert.ToDateTime(start, dtFormat);
            DateTime dt2 = Convert.ToDateTime(end, dtFormat);

            Course course = new Course();
            string coursesId = System.DateTime.Now.ToLongDateString();
            course.courseId = coursesId;
            course.courseName = name;
            course.courseStartTime = dt1;
            course.courseEndTime = dt2;
            course.coursePlace = address;
            course.courseTeacherName = teacher;
            course.courseCredit = 5;
            course.courseOutlineURL = "";
            course.courseRequireURL = "";
            course.courseTime = 0;
            course.maxNumPerGroup = 0;
            course.minNumPerGroup = 0;
            course.isEnd = false;

            db.courses.Add(course);
            db.SaveChanges();
            db = null;
        }

        internal void addCourse(string courseName, string courseStartTime, string courseEndTime, string coursePlace, string courseTeacherName, string courseCredit, string courseTime)
        {
            CourseDBContext db = new CourseDBContext();

            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            DateTime dt1 = Convert.ToDateTime(courseStartTime, dtFormat);
            DateTime dt2 = Convert.ToDateTime(courseEndTime, dtFormat);

            Course course = new Course();
            string coursesId = System.DateTime.Now.ToString();
            course.courseId = coursesId;
            course.courseName = courseName;
            course.courseStartTime = dt1;
            course.courseEndTime = dt2;
            course.coursePlace = coursePlace;
            course.courseTeacherName = courseTeacherName;

            /*
            int coursecredit = 0;
            try
            {
                coursecredit = Convert.ToInt32(courseCredit);
            }
            catch (Exception)
            {
                coursecredit = 0;
            }
            course.courseCredit = coursecredit;
            int coursetime = 0;
            try
            {
               coursetime = Convert.ToInt32(courseTime);

            }
            catch (Exception)
            {
                coursetime = 0;
            }
            course.courseTime = coursetime;
             */
            course.courseCredit = Convert.ToInt32(courseCredit);
            course.courseTime = Convert.ToInt32(courseTime);
            course.courseOutlineURL = "";
            course.courseRequireURL = "";
            course.maxNumPerGroup = 0;
            course.minNumPerGroup = 0;
            course.isEnd = false;

            db.courses.Add(course);
            db.SaveChanges();
            db = null;
        }

        public void addOutline(string courseid, string outline)
        {
            CourseDBContext db = new CourseDBContext();
            List<Course> clist = db.courses.ToList();
            foreach (Course course in clist)
            {
                if (course.courseId == courseid)
                {
                    course.courseOutlineURL = outline;
                }
            }
            int num = db.SaveChanges();
            db = null;
        }
        public void addRequire(string courseid, string require)
        {
            CourseDBContext db = new CourseDBContext();
            List<Course> clist = db.courses.ToList();
            foreach (Course course in clist)
            {
                if (course.courseId == courseid)
                {
                    course.courseRequireURL = require;
                }
            }
            int num = db.SaveChanges();
            db = null;
        }


        //寻找课程
        public Course findCourse(string courseId)
        {
            CourseDBContext cdb = new CourseDBContext();
            List<Course> list = cdb.courses.ToList();
            foreach(Course c in list)
            {
                if(c.courseId == courseId)
                {
                    return c;
                }
            }
            return null;
        }





    }

   


}