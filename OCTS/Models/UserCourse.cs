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
    [Table("userCourse")]
    public class UserCourse
    {
        public string courseId { get; set; }
        public string userId { get; set; }
        public int courseGrade { get; set; }
        public string groupId { get; set; }
        public Boolean isAgree { get; set; }
    }
    public class UserCourseDBContext : DbContext
    {
        public UserCourseDBContext() : base("name=OCTS") { }

        public DbSet<UserCourse> userCourses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserCourse>().HasKey(c => new { c.userId, c.courseId });//需要更改
        }
    }
}