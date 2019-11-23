using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class Teacher : User
    {
        public virtual ICollection<Course> Courses { get; set; }

        public Teacher() { this.Courses = new HashSet<Course>(); }

        public Teacher(string firstName, string lastName, string email, string username) : base(firstName, lastName, email, username)
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
                    var sqlEntry = dc.Teachers.FirstOrDefault(x => x.ID == this.ID);

                    if (sqlEntry == null)
                    {
                        // Insert the new user to the DB
                        dc.Teachers.Add(this);
                    }

                    if(sqlEntry != null)
                    {
                        sqlEntry.FirstName = this.FirstName;
                        sqlEntry.LastName = this.LastName;
                        sqlEntry.Username = this.Username;
                        sqlEntry.Courses = this.Courses;
                        sqlEntry.Email = this.Email;
                    }
                    
                    dc.SaveChanges();
                }
                success = true;
            }
            catch(Exception ex)
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