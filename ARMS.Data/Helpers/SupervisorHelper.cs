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
    public static class SupervisorHelper
    {
        // get teacher by id
        public static Supervisor GetById(int supervisorId)
        {
            var model = Get(supervisorId);
            return model;
        }

        // get teacher by username
        public static Supervisor GetByUsername(string email)
        {
            var model = Get(0, email);
            return model;
        }

        // get the actual teacher object
        private static Supervisor Get(int userId = 0, string email = null)
        {
            Supervisor model = null;

            try
            {
                using (var dc = new ArmsContext())
                {
                    // User id if given
                    if (userId > 0)
                    {
                        model = dc.Supervisors.Include(x => x.Course).FirstOrDefault(x => x.User.UserID == userId);
                    }
                    else if (email != null)
                    {
                        model = dc.Supervisors.Include(x => x.Course).FirstOrDefault(x => x.User.Email == email);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }

        // get all supervisors (users) for the specified course
        public static List<User> GetSupervisorsForCourse(int courseId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var users = dc.Supervisors.Where(ps => ps.CourseID == courseId).Select(ps => ps.User).ToList();
                    return users;
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return null;
        }
        
        // deletes a teacher with the targeted id
        public static void DeleteSupervisor(int userId, int courseId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var teacher = dc.Supervisors.Where(u => u.UserID == userId && u.CourseID == courseId).FirstOrDefault();
                    dc.Supervisors.Remove(teacher);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
        }

        // for testing purposes
        public static List<Supervisor> GetAllTeachers()
        {
            using (var dc = new ArmsContext())
            {
                var list = dc.Supervisors.ToList();
                return list;
            }
        }
    }
}