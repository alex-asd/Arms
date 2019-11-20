using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class Lecture
    {
        public DateTime Date { get; set; }
        public int LectureID { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }

        public Lecture() { this.Students = new HashSet<Student>(); this.Teachers = new HashSet<Teacher>(); }

        public Lecture(List<Teacher> teachers, Course course)
        {
            Date = DateTime.Now;
            this.Teachers = teachers;
            this.Course = course;
            this.CourseID = course.CourseID;
        }

        #region Database Interactions
        public bool Insert()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Lectures.FirstOrDefault(x => x.LectureID == this.LectureID);
                    // Insert the new user to the DB
                    dc.Lectures.Add(this);
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
                    var sqlEntry = dc.Lectures.FirstOrDefault(x => x.LectureID == this.LectureID);

                    sqlEntry.Date = this.Date;
                    sqlEntry.Course = this.Course;
                    sqlEntry.CourseID = this.Course.CourseID;
                    sqlEntry.Students = this.Students;
                    sqlEntry.Teachers = this.Teachers;

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
                    var sqlEntry = dc.Lectures.FirstOrDefault(x => x.LectureID == this.LectureID);
                    dc.Lectures.Remove(sqlEntry);

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