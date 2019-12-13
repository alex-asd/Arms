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
        // get teacher by supervisor id
        public static Supervisor GetById(int supervisorId)
        {
            var model = Get(supervisorId);
            return model;
        }

        // get teacher by username
        public static Supervisor GetByUserIdCourseId(int userId, int courseId)
        {
            var model = Get(0, userId, courseId);
            return model;
        }

        // get the actual teacher object
        private static Supervisor Get(int supervisorId = 0, int userId = 0, int courseId = 0)
        {
            Supervisor model = null;

            try
            {
                using (var dc = new ArmsContext())
                {
                    // User id if given
                    if (supervisorId > 0)
                    {
                        model = dc.Supervisors.Include(x => x.Course).FirstOrDefault(x => x.SupervisorID == supervisorId);
                    }
                    else if (userId > 0 && courseId > 0)
                    {
                        model = dc.Supervisors.Include(x => x.Course).FirstOrDefault(x => x.User.UserID == userId && x.CourseID == courseId);
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
            var users = new List<User>();
            try
            {
                using (var dc = new ArmsContext())
                {
                    users = dc.Supervisors.Where(s => s.CourseID == courseId).Select(s => s.User).ToList();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return users;
        }
        
        // deletes a teacher with the targeted id
        public static bool DeleteSupervisor(int userId, int courseId)
        {
            var success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var teacher = dc.Supervisors.Where(u => u.UserID == userId && u.CourseID == courseId).FirstOrDefault();
                    dc.Supervisors.Remove(teacher);

                    dc.SaveChanges();
                }
                success = true;
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return success;
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