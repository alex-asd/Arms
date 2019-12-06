using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using ARMS.Data.Helpers;
using ARMS.Data.Models;

namespace ARMS.Helpers
{
    public class APIUtils
    {
        public static bool CanChangeCourse(int courseId, ClaimsPrincipal principal)
        {
            var userEmail = APIUtils.GetCurrentUserEmail(principal);
            if (userEmail == null)
            {
                throw new Exception("Was unable to find the user name.");
            }


            var targetCourse = CourseHelper.GetById(courseId);
            var possible_creator = UserHelper.GetByEmail(userEmail);
            if (targetCourse != null)
            {
                return targetCourse.CreatorID == possible_creator.UserID;
            }


            return false;
        }


        private static string GetCurrentUserEmail(ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            return userName?.Value;
        }

        private static int GetUserFromClaim(ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var user = UserHelper.GetByEmail(email?.Value);
            return user.UserID;
        }
       
    }
}