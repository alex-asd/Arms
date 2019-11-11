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
    }
}