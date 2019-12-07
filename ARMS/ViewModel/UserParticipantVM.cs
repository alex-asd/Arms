using ARMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARMS.Data.Helpers;

namespace ARMS.ViewModel
{
    public class UserParticipantVM
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public decimal AttendancePercentage { get; set; }

        public UserParticipantVM()
        {
        }

        public UserParticipantVM(User user, Participant participant)
        {
            this.UserID = user.UserID;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.CourseID = participant.CourseID;
            this.AttendancePercentage = ParticipantHelper.GetParticipantAttendance(UserID, CourseID);
        }
    }
}