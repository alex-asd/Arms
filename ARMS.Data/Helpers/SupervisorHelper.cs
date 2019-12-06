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

        // get all participants (users) for the specified course
        public static List<User> GetParticipantsForCourse(int courseId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var participants = dc.Participants.Where(x => x.CourseID == courseId).ToList();

                    List<User> users = new List<User>();
                    foreach(var p in participants)
                    {
                        var user = dc.Users.FirstOrDefault(x => x.UserID == p.UserID);
                        users.Add(user);
                    }
                    return users;
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return null;
        }

        // get all supervisors (users) for the specified course
        public static List<User> GetSupervisorsForCourse(int courseId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var supervisors = dc.Supervisors.Where(x => x.CourseID == courseId).ToList();

                    List<User> users = new List<User>();
                    foreach (var p in supervisors)
                    {
                        var user = dc.Users.FirstOrDefault(x => x.UserID == p.UserID);
                        users.Add(user);
                    }
                    return users;
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return null;
        }

        // get all lectures for the specified course
        public static List<Lecture> GetLecturesForCourse(int courseId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var lectures = dc.Lectures.Where(x => x.CourseID == courseId).ToList();
                    
                    return lectures;
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return null;
        }

        // returns whether there is a teacher with such email in the DB
        //public static bool IsEmailRegistered(string email)
        //{
        //    using (var dc = new ArmsContext())
        //    {
        //        return !dc.Supervisors.Any(x => x.Email == email);
        //    }
        //}

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