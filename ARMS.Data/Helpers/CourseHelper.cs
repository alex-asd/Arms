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
            var list = new List<Course>();
            try
            {
                using (var dc = new ArmsContext())
                {
                    list = dc.Participants.Where(ps => ps.UserID == userId).Select(ps => ps.Course).Include(c => c.Creator).ToList();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return list;
        }

        // get courses for current teacher logged in
        public static List<Course> GetCoursesForSupervisor(int userId)
        {
            var list = new List<Course>();
            try
            {
                using (var dc = new ArmsContext())
                {
                    list = dc.Supervisors.Where(sp => sp.UserID == userId).Select(sp => sp.Course).Include(c => c.Creator).ToList();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return list;
        }

        // search for course containing the specified string
        public static List<Course> SearchCoursesFor(string searchString)
        {
            var list = new List<Course>();
            try
            {
                using (var dc = new ArmsContext())
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        list = dc.Courses.Where(c => c.CourseName.Contains(searchString)).Include(c => c.Creator).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return list;
        }

        // return true/false depending if the current user is a participant of the course
        public static bool IsStudentPartOfCourse(int userId, int courseId)
        {
            var boolean = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    boolean = dc.Participants.Any(p => p.UserID == userId && p.CourseID == courseId);
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return boolean;
        }
    }
}