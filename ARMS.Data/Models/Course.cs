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
        [Index(IsUnique = true)]
        [MaxLength(300)]
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

        public Course(string courseName, string courseDescription)
        {
            this.CourseName = courseName;
            this.CourseDescription = courseDescription;
        }

        #region Database Interactions
        public bool Upsert()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Courses.FirstOrDefault(x => x.CourseName == this.CourseName);

                    // Insert the new course to the DB
                    if (sqlEntry == null)
                    {
                        dc.Courses.Add(this);
                    }

                    // Update existing course
                    if(sqlEntry != null)
                    {
                        sqlEntry.CourseName = this.CourseName;
                        sqlEntry.CourseDescription = this.CourseDescription;
                        sqlEntry.Lectures = this.Lectures;
                        sqlEntry.Students = this.Students;
                        sqlEntry.Teachers = this.Teachers;
                    }

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
                    var sqlEntry = dc.Courses.FirstOrDefault(x => x.CourseName == this.CourseName);
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

        public int GetCourseID()
        {
            int id = 0;

            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Courses.FirstOrDefault(x => x.CourseName == this.CourseName);
                    id = sqlEntry.CourseID;
                }
            }
            catch(Exception ex)
            {
                var catchMsg = ex.Message;
            }

            return id;
        }
        #endregion
    }
}