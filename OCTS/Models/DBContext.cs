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
    public class DBContext:DbContext
    {
        public DBContext() : base("name=OCTS") { }
        public DbSet<User> users { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<TaskInformation> taskInformations { get; set; }
        public DbSet<Source> sources { get; set; }
        public DbSet<TermInformation> termInformations { get; set; }
        public DbSet<UserCourse> userCourses { get; set; }
        public DbSet<TaskSubmit> taskSubmits { get; set; }
        public DbSet<TeacherCourse> teacherCourses { get; set; }
        public DbSet<Group> groups { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserCourse>().HasKey(c => new { c.userId, c.courseId });
        }
    }
}