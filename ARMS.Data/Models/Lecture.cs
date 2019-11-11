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
    }
}