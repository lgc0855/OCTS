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
    [Table("taskSubmit")]
    public class TaskSubmit
    {
        public string groupId { get; set; }
        public string taskId { get; set; }
        public DateTime submitTime { get; set; }//提交时间
        public string submitterId { get; set; }
        public string submitTimes { get; set; }//提交次数
        public int taskGrade { get; set; }
        public string taskComment { get; set; }//作业评论

    }

    public class TaskSubmitDBContext : DbContext
    {
        public TaskSubmitDBContext() : base("name=OCTS") { }

        public DbSet<TaskSubmit> taskSubmits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskSubmit>().HasKey(c => c.groupId);//需要改
        }
    }
}