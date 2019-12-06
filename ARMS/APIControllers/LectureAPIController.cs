using System;
using ARMS.Data.Helpers;
using ARMS.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Xml;
using ARMS.Data;

namespace ARMS.APIControllers
{
    [RoutePrefix("lectures")]
    public class LectureApiController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("get")]
        public IHttpActionResult GetById(int id)
        {
            var lecture = LectureHelper.GetById(id);
            return Ok<Lecture>(lecture);
        }


        [HttpPost]
        [Authorize]
        [Route("add")]
        public IHttpActionResult AddLecture(Lecture lecture)
        {
            lecture.Insert(BonusEnum.UpsertType.Insert);
            var id = lecture.GetLectureID();
            return Ok(id);
        }

        [HttpPost]
        [Authorize]
        [Route("update")]
        public IHttpActionResult UpdateLecture(Lecture lecture)
        {
            lecture.Update();
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("forstudent")]
        public IHttpActionResult GetForStudent(int id)
        {
            var curr_user = UserHelper.GetById(id);
            using (var dc = new ArmsContext())
            {
                var participations = dc.Participants.Where(x => x.ParticipantID == curr_user.UserID)
                    .Select(x => x.CourseID);
                var participating_courses =
                    dc.Courses.Where(x => participations.Contains(x.CourseID)).Select(x => x.CourseID);
                var courses_lectures = dc.Lectures.Where(x => participating_courses.Contains(x.CourseID));
                return Ok<List<Lecture>>(courses_lectures.ToList());
            }
        }


        [HttpGet]
        [Authorize]
        [Route("forteacher")]
        public IHttpActionResult GetForTeacher(int id)
        {
            var curr_user = UserHelper.GetById(id);
            using (var dc = new ArmsContext())
            {
                var supervisions = dc.Supervisors.Where(x => x.SupervisorID == curr_user.UserID)
                    .Select(x => x.CourseID);
                var supervised_courses =
                    dc.Courses.Where(x => supervisions.Contains(x.CourseID)).ToList();
                return Ok();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("for-course")]
        public IHttpActionResult GetForCourse(int id)
        {
            var course = CourseHelper.GetById(id);
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var lectures = dc.Lectures.Include(x => x.Course).Where(x => x.CourseID == course.CourseID);
                return Ok(lectures.ToList());
            }
        }

        [HttpGet]
        [Authorize]
        [Route("activeforstudent")]
        public IHttpActionResult GetActiveLectureForStudent(int id)
        {
            var curr_user = UserHelper.GetById(id);
            using (var dc = new ArmsContext())
            {
                var participations = dc.Participants.Where(x => x.ParticipantID == curr_user.UserID)
                    .Select(x => x.CourseID);
                var participating_courses =
                    dc.Courses.Where(x => participations.Contains(x.CourseID)).Select(x => x.CourseID);
                var active_lectures = dc.Lectures.Where(x =>
                    participating_courses.Contains(x.CourseID) && DateTime.Now.CompareTo(x.From) >= 0 &&
                    DateTime.Now.CompareTo(x.To) <= 0);
                return Ok<List<Lecture>>(active_lectures.ToList());
            }
        }

        [HttpGet]
        [Authorize]
        [Route("activeforteacher")]
        public IHttpActionResult GetActiveLectureForTeacher(int id)
        {
            var curr_user = UserHelper.GetById(id);
            using (var dc = new ArmsContext())
            {
                var supervisions = dc.Supervisors.Where(x => x.SupervisorID == curr_user.UserID)
                    .Select(x => x.CourseID);
                var supervised_courses =
                    dc.Courses.Where(x => supervisions.Contains(x.CourseID)).Select(x => x.CourseID);
                var active_lectures = dc.Lectures.Where(x =>
                    supervised_courses.Contains(x.CourseID) && DateTime.Now.CompareTo(x.From) >= 0 &&
                    DateTime.Now.CompareTo(x.To) <= 0);
                return Ok<List<Lecture>>(active_lectures.ToList());
            }
        }
    }
}