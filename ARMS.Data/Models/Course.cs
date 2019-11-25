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

        public int CreatorID { get; set; }
        [ForeignKey("CreatorID")]
        public virtual User Creator { get; set; }

        public Course() { }

        public Course(string courseName, string courseDescription, int creatorID)
        {
            this.CourseName = courseName.ToLower();
            this.CourseDescription = courseDescription;
            this.CreatorID = creatorID;
        }

        #region Database Interactions
        public bool Upsert()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Courses.FirstOrDefault(x => x.CourseName == this.CourseName.ToLower());

                    // Insert the new course to the DB
                    if (sqlEntry == null)
                    {
                        dc.Courses.Add(this);
                    }

                    // Update existing course
                    if (sqlEntry != null)
                    {
                        sqlEntry.CourseName = this.CourseName;
                        sqlEntry.CourseDescription = this.CourseDescription;
                        sqlEntry.CreatorID = this.CreatorID;
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
                    return sqlEntry.CourseID;
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }

            return id;
        }
        #endregion
    }
}