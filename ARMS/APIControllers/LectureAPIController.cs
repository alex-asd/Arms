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
using ARMS.Helpers;
using ARMS.ViewModel;

namespace ARMS.APIControllers
{
    [RoutePrefix("api/lectures")]
    public class LectureApiController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("get")]
        public IHttpActionResult GetById(int id)
        {
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                return Ok<Lecture>(dc.Lectures.FirstOrDefault(x => x.LectureID == id));
            }
        }


        [HttpPost]
        [Authorize]
        [Route("add")]
        public IHttpActionResult AddLecture([FromBody] Lecture lecture)
        {
            lecture.Insert(BonusEnum.UpsertType.Insert);
            var id = lecture.GetLectureID();
            return Ok(id);
        }

        [HttpPost]
        [Authorize]
        [Route("update")]
        public IHttpActionResult UpdateLecture([FromBody] Lecture lecture)
        {
            lecture.Update();
            return Ok(new ApiCallbackMessage("Success", true));
        }

        [HttpPost]
        [Authorize]
        [Route("reschedule")]
        public IHttpActionResult RescheduleLecture(int lecture_id, int times)
        {
            var lecture = LectureHelper.GetById(lecture_id);
            for (int i = 0; i < times; i++)
            {
                var new_lec = new Lecture(lecture.From.AddDays(7 * (i + 1)), lecture.To.AddDays(7 * (i + 1)),
                    lecture.CourseID);
                new_lec.Insert();
            }

            return Ok(new ApiCallbackMessage("Success", true));
        }


        [HttpPost]
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteLecture(int lecture_id)
        {
            var lecture = LectureHelper.GetById(lecture_id);
            var deleted = lecture.Delete();
            return Ok(new ApiCallbackMessage(deleted ? "Success" : "Falied", deleted));
        }

        [HttpGet]
        [Authorize]
        [Route("forstudent")]
        public IHttpActionResult GetForStudent(int id)
        {
            var curr_user = UserHelper.GetById(id);
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var participations = dc.Participants
                    .Where(x => x.UserID == curr_user.UserID && x.ParticipantStatus == "active")
                    .Select(x => x.CourseID).ToList();
                var courses_lectures = dc.Lectures.Include(x => x.Course).Where(x =>
                    participations.Contains(x.CourseID) && x.To.CompareTo(DateTime.Now) >= 0);
                return Ok<List<Lecture>>(courses_lectures.ToList());
            }
        }


        [HttpGet]
        [Authorize]
        [Route("forteacher")]
        public IHttpActionResult GetForTeacher()
        {
            var curr_user = UserHelper.GetById(APIUtils.GetUserFromClaim(ClaimsPrincipal.Current));
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var supervisions = dc.Supervisors.Where(x => x.UserID == curr_user.UserID)
                    .Select(x => x.CourseID).ToList();
                var upcmoning_lectures = dc.Lectures.Include(x => x.Course)
                    .Where(x => supervisions.Contains(x.CourseID) && x.To.CompareTo(DateTime.Now) >= 0)
                    .ToList();
                return Ok(upcmoning_lectures);
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
        public IHttpActionResult GetActiveLectureForStudent()
        {
            var curr_user = UserHelper.GetById(APIUtils.GetUserFromClaim(ClaimsPrincipal.Current));
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var participations = dc.Participants.Where(x => x.UserID == curr_user.UserID)
                    .Select(x => x.CourseID).ToList();
                var active_lectures = dc.Lectures.Include(x => x.Course).FirstOrDefault(x =>
                    participations.Contains(x.CourseID) && x.To.CompareTo(DateTime.Now) >= 0 &&
                    x.From.CompareTo(DateTime.Now) <= 0);
                return Ok<Lecture>(active_lectures);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("activeforteacher")]
        public IHttpActionResult GetActiveLectureForTeacher()
        {
            var curr_user = UserHelper.GetById(APIUtils.GetUserFromClaim(ClaimsPrincipal.Current));
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                var supervisions = dc.Supervisors.Where(x => x.UserID == curr_user.UserID)
                    .Select(x => x.CourseID).ToList();
                var active_lectures = dc.Lectures.Include(x => x.Course).FirstOrDefault(x =>
                    supervisions.Contains(x.CourseID) && x.To.CompareTo(DateTime.Now) >= 0 &&
                    x.From.CompareTo(DateTime.Now) <= 0);
                return Ok<Lecture>(active_lectures);
            }
        }
    }
}