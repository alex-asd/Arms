using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using ARMS.Helpers;
using ARMS.Data.Helpers;
using ARMS.Data.Models;
using ARMS.Data;

namespace ARMS.APIControllers
{
    [RoutePrefix("courses")]
    public class CourseApiController : ApiController
    {
//        [HttpGet]
//        [Authorize]
//        [Route("get")]
//        public IHttpActionResult GetById(int id)
//        {
//            Course course = CourseHelper.GetById(id);
//            return Ok(course);
//        }
//
//        [HttpGet]
//        [Authorize]
//        [Route("forstudent")]
//        public IHttpActionResult GetForStudent(int id)
//        {
////            Student student = StudentHelper.GetById(id);
////            var studentCourses = student.Courses.ToList();
//
//            return Ok();
//        }
//
//        [HttpGet]
//        [Authorize]
//        [Route("searchbyname")]
//        public IHttpActionResult GetByName(string name)
//        {
//            List<Course> resultCourses = null;
//            using (var dc = new ArmsContext())
//            {
//                var searchCourses = dc.Courses.Where(course => course.CourseName.ToLower().Contains(name.ToLower()));
//                resultCourses = searchCourses.ToList();
//                return Ok(resultCourses);
//            }
//
//        }
//
//        [HttpPost]
//        [Authorize]
//        [Route("update")]
//        public IHttpActionResult UpdateCourse([FromBody] Course courseToUpdate)
//        {
//            if (APIUtils.CanChangeCourse(courseToUpdate.CourseID, ClaimsPrincipal.Current))
//            {
//                courseToUpdate.Upsert();
//                return Ok();
//            }
//
//            return Unauthorized();
//        }
//
//        [HttpPost]
//        [Authorize]
//        [Route("add")]
//        public IHttpActionResult AddCourse([FromBody] Course courseToAdd)
//        {
//            bool success = false;
//            success = courseToAdd.Upsert();
//
//            if (success)
//            {
//                return Ok();
//            }
//
//            return BadRequest();
//        }
    }
}