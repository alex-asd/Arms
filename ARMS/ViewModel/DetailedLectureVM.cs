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
    public class DetailedLectureVM
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int LectureID { get; set; }
        [Display(Name = "Check-in")]
        public bool CheckInEnabled { get; set; }

        public string CourseName { get; set; }
        public int CourseID { get; set; }

        public List<User> Attendees { get; set; }

        public DetailedLectureVM(Lecture lecture)
        {
            this.From = lecture.From;
            this.To = lecture.To;
            this.LectureID = lecture.LectureID;
            this.CheckInEnabled = lecture.CheckInEnabled;
            this.CourseID = lecture.CourseID;
        }
    }
}