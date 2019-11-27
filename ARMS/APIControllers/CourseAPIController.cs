using ARMS.Data.Helpers;
using ARMS.Data.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using ARMS.Data;
using ARMS.Helpers;

namespace ARMS.APIControllers
{
    [RoutePrefix("courses")]
    public class CourseApiController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("get")]
        public IHttpActionResult GetById(int id)
        {
            Course course = CourseHelper.GetById(id);
            return Ok(course);
        }

        [HttpGet]
        [Authorize]
        [Route("foruser")]
        public IHttpActionResult GetForUser(int id)
        {
            var curr_user = UserHelper.GetById(id);
            using (var dc = new ArmsContext())
            {
                var participations = dc.Participants.Where(x => x.ParticipantID == curr_user.UserID)
                    .Select(x => x.CourseID);
                var participating_courses =
                    dc.Courses.Where(x => participations.Contains(x.CourseID));
                return Ok<List<Course>>(participating_courses.ToList());
            }

            return Ok();
        }


        [HttpGet]
        [Authorize]
        [Route("searchbyname")]
        public IHttpActionResult GetByName(string name)
        {
            List<Course> resultCourses = null;
            using (var dc = new ArmsContext())
            {
                var searchCourses = dc.Courses.Where(course => course.CourseName.ToLower().Contains(name.ToLower()));
                resultCourses = searchCourses.ToList();
                return Ok(resultCourses);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("update")]
        public IHttpActionResult UpdateCourse([FromBody] Course courseToUpdate)
        {
            if (!APIUtils.CanChangeCourse(courseToUpdate.CourseID, ClaimsPrincipal.Current))
            {
                return Unauthorized();
            }

            courseToUpdate.Update();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("add")]
        public IHttpActionResult AddCourse([FromBody] Course courseToAdd)
        {
            bool success = false;
            success = courseToAdd.Insert();

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        [Route("add-supervisor-to-course")]
        public IHttpActionResult AddSupervisorToCourse([FromBody] Course courseToAddTo, [FromBody] User userToAdd)
        {
            if (!APIUtils.CanChangeCourse(courseToAddTo.CourseID, ClaimsPrincipal.Current))
            {
                return Unauthorized();
            }
            
            using (var dc = new ArmsContext())
            {
                dc.Supervisors.Add(new Supervisor(userToAdd.UserID, courseToAddTo.CourseID));
                dc.SaveChanges();
                return Ok();
            }
        }
    }
}