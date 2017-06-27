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
    [Table("teacherCourse")]
    public class TeacherCourse
    {
        public string courseId { get; set; }
        public string userId { get; set; }
    }
    public class TeacherCourseDBContext : DbContext
    {
        public TeacherCourseDBContext() : base("name=OCTS") { }

        public DbSet<TeacherCourse> teacherCourses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeacherCourse>().HasKey(c => c.userId);//需要改
        }
    }
}