using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARMS.Data.Helpers;
using ARMS.Data;
using ARMS.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ARMS.ViewModel
{
    public class DetailedCourseVM
    {
        [Display(Name = "Course ID")]
        public int CourseID { get; set; }
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Display(Name = "Course Description")]
        public string CourseDescription { get; set; }
        public User Creator { get; set; }
        public int CreatorID { get; set; }

        public List<User> Participants { get; set; }
        public List<Lecture> Lectures { get; set; }
        public List<User> Supervisors { get; set; }

        public DetailedCourseVM() { }

        public DetailedCourseVM(Course course)
        {
            this.CourseID = course.CourseID;
            this.CreatorID = course.CreatorID;
            this.CourseName = course.CourseName;
            this.CourseDescription = course.CourseDescription;
            this.Creator = course.Creator;
        }
    }
}