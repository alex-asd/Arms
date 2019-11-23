using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class Student : User
    {
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }

        public Student()
        {
            this.Courses = new HashSet<Course>();
            this.Lectures = new HashSet<Lecture>();
        }

        public Student(string firstName, string lastName, string email, string username) : base(firstName, lastName, email, username)
        {

        }

        #region Database Interactions
        public bool Upsert()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Students.FirstOrDefault(x => x.ID == this.ID);

                    // Insert new student to DB
                    if (sqlEntry == null)
                    {
                        // Insert the new user to the DB
                        dc.Students.Add(this);
                    }

                    // Update existing entry
                    if(sqlEntry != null)
                    {
                        sqlEntry.FirstName = this.FirstName;
                        sqlEntry.LastName = this.LastName;
                        sqlEntry.Username = this.Username;
                        sqlEntry.Lectures = this.Lectures;
                        sqlEntry.Courses = this.Courses;
                        sqlEntry.Email = this.Email;
                    }
                    dc.SaveChanges();
                    success = true;
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
                    var sqlEntry = dc.Students.FirstOrDefault(x => x.ID == this.ID);
                    dc.Students.Remove(sqlEntry);

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
