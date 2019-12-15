using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARMS.Data;
using ARMS.Data.Models;
using ARMS.Data.Helpers;

namespace ARMS.APIControllers
{
    [Authorize]
    public class UtilController : ApiController
    {

        [HttpGet]
        [Route("API/Util/AddSupervisor")]
        public void AddSupervisor(int courseId, string email)
        {
            var user = UserHelper.GetByEmail(email);
            if (user == null)
                return;
            if (user.Type != "teacher")
                return;
            var supervisor = new Supervisor(user.UserID, courseId);
            supervisor.Insert();
        }

        [Route("API/Util/DeleteSupervisor")]
        [HttpGet]
        public void DeleteSupervisor(int userId, int courseId)
        {
            SupervisorHelper.DeleteSupervisor(userId, courseId);
        }

        [HttpGet]
        [Route("API/Util/AddParticipant")]
        public void AddParticipant(int courseId, string email)
        {
            var user = UserHelper.GetByEmail(email);
            if (user == null)
                return;
            if (user.Type != "student")
                return;
            var participant = new Participant(user.UserID, courseId, Participant.STATUS_ACTIVE);
            participant.Insert();
        }

        [HttpGet]
        [Route("API/Util/ApproveParticipant")]
        public void ApproveParticipant(int userId, int courseId)
        {
            var participant = ParticipantHelper.GetById(userId, courseId);
            if (participant == null)
                return;
            participant.ParticipantStatus = Participant.STATUS_ACTIVE;
            participant.Update();
        }

        [HttpGet]
        [Route("API/Util/DeleteParticipant")]
        public void DeleteParticipant(int userId, int courseId)
        {
            ParticipantHelper.DeleteParticipant(userId, courseId);
        }

        [HttpGet]
        [Route("API/Util/AddAttendee")]
        public void AddAttendee(int lectureId, string email)
        {
            var user = UserHelper.GetByEmail(email);
            if (user == null)
                return;
            var attendee = new Attendee(user.UserID, lectureId, "");
            attendee.Insert();
        }

        [HttpGet]
        [Route("API/Util/DeleteAttendee")]
        public void DeleteAttendee(int lectureId, int userId)
        {
            AttendeeHelper.DeleteAttendee(lectureId, userId);
        }

        [HttpGet]
        [Route("API/Util/AddLecture")]
        public void AddLecture(DateTime from, DateTime to, int courseId)
        {
            var lecture = new Lecture(from, to, courseId);
            lecture.Insert();
        }

        [HttpGet]
        [Route("API/Util/DeleteLecture")]
        public void DeleteLecture(int lectureId)
        {
            LectureHelper.DeleteLecture(lectureId);
        }
    }
}
