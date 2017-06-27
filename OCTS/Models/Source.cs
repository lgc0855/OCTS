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
    [Table("Source")]
    public class Source
    {
        public string sourceId { get; set; }
        public string sourceURL { get; set; }
        public string sourceName { get; set; }
        public DateTime sourceTime { get; set; }
        public string sourceSubmitter { get; set; }
        public string courseId { get; set; }
    }

    public class SourceDBContext : DbContext
    {
        public SourceDBContext() : base("name=OCTS") { }

        public DbSet<Source> sources { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Source>().HasKey(c => c.sourceId);
        }
    }
}