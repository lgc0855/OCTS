using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OCTS.Models
{
     [Table("course")]
    public class Course
    {
         public string courseId { get; set; }
         public string courseName { get; set; }
         public DateTime courseStartTime { get; set; }
         public DateTime courseEndTime { get; set; }
         public string courseRequireURL { get; set; }
         public string courseOutlineURL { get; set; }
         public string courseTeacherName { get; set; }
         //public string courseSourceURL { get; set; }
         public int courseCredit { get; set; }
         public string coursePlace { get; set; }
         public int maxNumPerGroup { get; set; }//团队最大人数
         public int minNumPerGroup { get; set; }//团队最小人数
         //public int maxStudentNum { get; set; }//课程最大学生数
         public int courseTime { get; set; }
         public Boolean isEnd { get; set; }

    }
     public class CourseDBContext : DbContext
     {
         public CourseDBContext() : base("name=OCTS") { }

         public DbSet<Course> courses { get; set; }

         protected override void OnModelCreating(DbModelBuilder modelBuilder)
         {
             base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<Course>().HasKey(c => c.courseId);
         }
     }
}