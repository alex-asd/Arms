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

namespace ARMS.Helpers
{
    public static class TeacherHelper
    {
        // get teacher by id
        public static Teacher GetById(int teacherId)
        {
            var model = Get(teacherId);
            return model;
        }

        // get teacher by username
        public static Teacher GetByUsername(string username)
        {
            var lowUsername = username.ToLower();
            var model = Get(0, lowUsername);
            return model;
        }

        // get the actual teacher object
        private static Teacher Get(int teacherId = 0, string username = null)
        {
            Teacher model = null;

            try
            {
                using (var dc = new ArmsContext())
                {
                    // User id if given
                    if (teacherId > 0)
                    {
                        model = dc.Teachers.Include(x => x.Courses).Include(x => x.Lectures).FirstOrDefault(x => x.ID == teacherId);
                    }
                    else if (username != null)
                    {
                        model = dc.Teachers.Include(x => x.Courses).Include(x => x.Lectures).FirstOrDefault(x => x.Username == username);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }

        // returns whether there is a teacher with such username in the DB
        public static bool IsUsernameRegistered(string username)
        {
            var lowerUsername = username.ToLower();
            using (var dc = new ArmsContext())
            {
                return !dc.Teachers.Any(x => x.Username == lowerUsername);
            }
        }

        // returns whether there is a teacher with such email in the DB
        public static bool IsEmailRegistered(string email)
        {
            using (var dc = new ArmsContext())
            {
                return !dc.Teachers.Any(x => x.Email == email);
            }
        }

        // deletes a Teacher with the targeted username
        public static void DeleteTeacher(string username)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var Teacher = dc.Teachers.Where(u => u.Username == username.ToLower()).FirstOrDefault();
                    dc.Teachers.Remove(Teacher);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
        }

        // for testing purposes
        public static List<Teacher> GetAllTeachers()
        {
            using (var dc = new ArmsContext())
            {
                var list = dc.Teachers.Include(x => x.Courses).Include(x => x.Lectures).ToList();
                return list;
            }
        }
    }
}