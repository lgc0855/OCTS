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
    [Table("taskInformation")]
    public class TaskInformation
    {
        public string taskId { get; set; }
        public string taskStartTime { get; set; }
        public string taskEndTime { get; set; }
        public string taskMaxSubmitTime { get; set; }
        public string taskPercent { get; set; }
        public string taskRequire { get; set; }
        public string taskSubmitter { get; set; }
    }
    public class TaskInformationDBContext : DbContext
    {
        public TaskInformationDBContext() : base("name=OCTS") { }

        public DbSet<TaskInformation> taskInformations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskInformation>().HasKey(c => c.taskId);
        }
    }
}