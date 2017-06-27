using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }

   


}