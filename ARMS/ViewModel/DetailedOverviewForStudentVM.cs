using ARMS.Data.Models;
using ARMS.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ARMS.ViewModel
{
    public class DetailedOverviewForStudentVM
    {
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Type { get; set; }
        public int CourseID { get; set; }
        [Display(Name = "Course")]
        public string CourseName { get; set; }
        [Display(Name = "Description")]
        public string CourseDescription { get; set; }
        public int CreatorID { get; set; }
        public virtual User Creator { get; set; }
        public int UserID { get; set; }

        public List<Lecture> AttendedLectures { get; set; }
        public List<Lecture> AllLectures { get; set; }

        public DetailedOverviewForStudentVM() { }

        public DetailedOverviewForStudentVM(User user, Course course)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.UserID = user.UserID;
            this.Email = user.Email;
            this.Type = user.Type;
            this.Creator = course.Creator;
            this.CreatorID = course.CreatorID;
            this.CourseID = course.CourseID;
            this.CourseName = course.CourseName;
            this.CourseDescription = course.CourseDescription;
        }

        public static DetailedOverviewForStudentVM CreateDetailedOverviewForStudentVM(User user, Course course)
        {
            var vm = new DetailedOverviewForStudentVM(user, course)
            {
                AttendedLectures = LectureHelper.GetAllAttendedLecturesForStudent(course.CourseID, user.UserID),
                AllLectures = LectureHelper.GetLecturesForCourse(course.CourseID)
            };

            return vm;
        }
    }
}