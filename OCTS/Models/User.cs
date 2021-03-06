﻿using System;
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
    [Table("user")]
    public class User
    {
        public enum userTypeNum { student = 3, teacher = 2,jiaowu = 1 };
        public string userId { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public userTypeNum userType { get; set; }
        public string userEmail { get; set; }
        public string userPhone { get; set; }
        //public string userGroupId { get; set; }
        public string userSex { get; set; }

        public bool setUserType(string  type)
        {
            switch (type)
            {
                case "student":
                    userType = userTypeNum.student;
                    break;
                case "teacher":
                    userType = userTypeNum.teacher;
                    break;
                case "jiaowu":
                    userType = userTypeNum.teacher;
                    break;
                default:
                    return false;
            }
            return true;
        }
    }



    public class UserDBContext : DbContext
    {
        public UserDBContext() : base("name=OCTS") { }

        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(c => c.userId);
        }
    }
}