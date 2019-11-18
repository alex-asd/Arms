﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class Teacher : User
    {
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }

        public Teacher() { this.Courses = new HashSet<Course>(); this.Lectures = new HashSet<Lecture>(); }

        public Teacher(string firstName, string lastName, string email, string password) : base(firstName, lastName, email, password)
        {

        }

        #region Database Interactions
        public bool Insert()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Teachers.FirstOrDefault(x => x.ID == this.ID);
                    // Insert the new user to the DB
                    dc.Teachers.Add(this);
                }
            }
            catch(Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return success;
        }

        public bool Update()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Teachers.FirstOrDefault(x => x.ID == this.ID);
                    
                    sqlEntry.FirstName = this.FirstName;
                    sqlEntry.LastName = this.LastName;
                    sqlEntry.Password = this.Password;
                    sqlEntry.Lectures = this.Lectures;
                    sqlEntry.Courses = this.Courses;
                    sqlEntry.Email = this.Email;

                    dc.SaveChanges();
                }
                success = true;
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return success;
        }

        public bool Delete()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Teachers.FirstOrDefault(x => x.ID == this.ID);
                    dc.Teachers.Remove(sqlEntry);

                    dc.SaveChanges();
                }
                success = true;
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return success;
        }
        #endregion
    }
}