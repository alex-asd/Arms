﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using ARMS.Data.Models;
using ARMS.Data;

namespace ARMS.Helpers
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
            var lowCourseName = courseName.ToLower();
            var model = Get(0, lowCourseName);
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
                        model = dc.Courses.Include(x => x.Students).Include(x => x.Lectures).Include(x => x.Teachers).FirstOrDefault(x => x.CourseID == courseId);
                    }
                    else if (courseName != null)
                    {
                        model = dc.Courses.Include(x => x.Students).Include(x => x.Lectures).Include(x => x.Teachers).FirstOrDefault(x => x.CourseName == courseName);
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
                var list = dc.Courses.Include(x => x.Students).Include(x => x.Lectures).Include(x => x.Teachers).ToList();
                return list;
            }
        }
    }
}