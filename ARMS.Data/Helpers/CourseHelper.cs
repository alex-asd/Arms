using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using ARMS.Data.Models;
using ARMS.Data;

namespace ARMS.Data.Helpers
{
    public static class CourseHelper
    {
        // get course by its id
        public static Course GetById(int courseId)
        {
            var model = Get(courseId);
            return model;
        }

        // get course by its name
        public static Course GetByName(string courseName)
        {
            var model = Get(0, courseName);
            return model;
        }

        // getting the actual course from db
        private static Course Get(int courseId = 0, string courseName = null)
        {
            Course model = null;

            try
            {
                using (var dc = new ArmsContext())
                {
                    // User id if given
                    if (courseId > 0)
                    {
                        model = dc.Courses.Include(x => x.Creator).FirstOrDefault(x => x.CourseID == courseId);
                    }
                    else if (courseName != null)
                    {
                        model = dc.Courses.Include(x => x.Creator).FirstOrDefault(x => x.CourseName == courseName);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }

        // returns whether there is a Course with such courseName in the DB
        public static bool Exists(string courseName)
        {
            var lowerCourseName = courseName.ToLower();
            using (var dc = new ArmsContext())
            {
                return !dc.Courses.Any(u => u.CourseName == lowerCourseName);
            }
        }

        // deletes a Course with the targeted name
        public static void DeleteCourse(string courseName)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var Course = dc.Courses.Where(u => u.CourseName == courseName.ToLower()).FirstOrDefault();
                    dc.Courses.Remove(Course);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
        }

        // for testing purposes
        public static List<Course> GetAllCourses()
        {
            using (var dc = new ArmsContext())
            {
                var list = dc.Courses.Include(x => x.Creator).ToList();
                return list;
            }
        }

        // get courses for current student logged in
        public static List<Course> GetCoursesForParticipant(int userId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var list = dc.Participants.Where(ps => ps.UserID == userId).Select(ps => ps.Course).Include(c => c.Creator).ToList();
                    
                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return null;
        }

        // get courses for current teacher logged in
        public static List<Course> GetCoursesForSupervisor(int userId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var list = dc.Supervisors.Where(ps => ps.UserID == userId).Select(ps => ps.Course).Include(c => c.Creator).ToList();
                    
                    return list.ToList();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return null;
        }
    }
}