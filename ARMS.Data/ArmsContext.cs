﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ARMS.Data.Models;

namespace ARMS.Data
{
    public class ArmsContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecture> Lectures { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendee>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Participant>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Supervisor>().HasRequired(c => c.User).WithMany().WillCascadeOnDelete(false);
        }
    }
}