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
    [Table("group")]
    public class Group
    {
        public enum GroupState{allow=1,forbid=2,approved=3};
        public string groupId { get; set; }
        public string groupLeaderId { get; set; }
        public string numOfStudent { get; set; }
        public string groupName { get; set; }
        public string courseId { get; set; }
        public GroupState groupState { get; set; }

    }
    public class GroupDBContext : DbContext
    {
        public GroupDBContext() : base("name=OCTS") { }

        public DbSet<Group> groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Group>().HasKey(c => c.groupId);
        }
    }
}