using ARMS.Data.Helpers;
using ARMS.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using ARMS.Data;
using ARMS.Helpers;
using ARMS.ViewModel;

namespace ARMS.APIControllers
{
    [RoutePrefix("api/courses")]
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
        public IHttpActionResult GetForUser()
        {
            var user_id = APIUtils.GetUserFromClaim(ClaimsPrincipal.Current);
            var curr_user = UserHelper.GetById(user_id);
            using (var dc = new ArmsContext())
            {
                dc.Configuration.LazyLoadingEnabled = false;
                if (curr_user.Type == "student")
                {
                    var participations = dc.Participants.Include(x => x.Course).Include(x => x.Course.Creator)
                        .Include(x => x.User)
                        .Where(x => x.UserID == curr_user.UserID);
                    return Ok(participations.ToList());
                }

                if (curr_user.Type == "teacher")
                {
                    var supervisions = dc.Supervisors.Include(x => x.Course).Include(x => x.Course.Creator)
                        .Include(x => x.User)
                        .Where(x => x.UserID == curr_user.UserID);
                    return Ok(supervisions.ToList());
                }
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
                var searchCourses = dc.Courses.Include(m => m.Creator)
                    .Where(course => course.CourseName.ToLower().Contains(name.ToLower()));
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
            return Ok(new ApiCallbackMessage("Success", true));
        }

        [HttpPost]
        [Authorize]
        [Route("add")]
        public IHttpActionResult AddCourse([FromBody] Course courseToAdd)
        {
            bool success = false;
            success = courseToAdd.Insert();
            var id = courseToAdd.GetCourseID();
            Supervisor sup = new Supervisor(courseToAdd.CreatorID, id);
            sup.Insert(BonusEnum.UpsertType.Insert);
            if (success)
            {
                return Ok(id);
            }

            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteCourse([FromBody] Course courseToDelete)
        {
            bool success = false;
            APIUtils.CanChangeCourse(courseToDelete.CourseID, ClaimsPrincipal.Current);
            success = courseToDelete.Delete();
            if (success)
            {
                return Ok(new ApiCallbackMessage("", success));
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
                return Ok(new ApiCallbackMessage("Success", true));
            }
        }
    }
}