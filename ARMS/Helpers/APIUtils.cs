using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using ARMS.Data.Helpers;

namespace ARMS.Helpers
{
    public class APIUtils
    {
        public static bool CanChangeCourse(int courseId, ClaimsPrincipal principal)
        {
            var userName = APIUtils.GetUserUsername(principal);
            if (userName == null)
            {
                throw new Exception("Was unable to find the user name.");
            }

            var teacher = TeacherHelper.GetByUsername(userName);
//
//            var targetCourse = CourseHelper.GetById(courseId);
//            if (targetCourse != null)
//            {
//                return targetCourse.Teachers.Count(x => x.Username == teacher.Username) == 1;
//            }


            return false;
        }


        private static string GetUserUsername(ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userName?.Value;
        }
    }
}