using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ARMS.Data.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        [Required]
        public string CourseName { get; set; }
        [MaxLength(200)]
        public string CourseDescription { get; set; }

        public ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }

        public Course()
        {
            this.Students = new HashSet<Student>();
            this.Teachers = new HashSet<Teacher>();
        }

        public Course(int courseID, string courseName, string courseDescription)
        {
            this.CourseID = courseID;
            this.CourseName = courseName;
            this.CourseDescription = courseDescription;
        }

        #region Database Interactions
        public bool Insert()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Courses.FirstOrDefault(x => x.CourseID == this.CourseID);
                    // Insert the new user to the DB
                    dc.Courses.Add(this);
                }
            }
            catch (Exception ex)
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
                    var sqlEntry = dc.Courses.FirstOrDefault(x => x.CourseID == this.CourseID);

                    sqlEntry.CourseName = this.CourseName;
                    sqlEntry.CourseDescription = this.CourseDescription;

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
                    var sqlEntry = dc.Courses.FirstOrDefault(x => x.CourseID == this.CourseID);
                    dc.Courses.Remove(sqlEntry);

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