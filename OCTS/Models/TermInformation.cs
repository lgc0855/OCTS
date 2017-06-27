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
    [Table("termInformation")]
    public class TermInformation
    {
        public int termId { get; set; }
        public DateTime termStartTime { get; set; }
        public DateTime termEndTime { get; set; }
        public int termWeeks { get; set; }
    }
    public class TermInformationDBContext : DbContext
    {
        public TermInformationDBContext() : base("name=OCTS") { }

        public DbSet<TermInformation> termInformations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TermInformation>().HasKey(c => c.termId);
        }
    }
}