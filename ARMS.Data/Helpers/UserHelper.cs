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
    public static class UserHelper
    {
        // get user by id
        public static User GetById(int userId)
        {
            var model = Get(userId);
            return model;
        }
      
        // get user by email
        public static User GetByEmail(string email)
        {
            var model = Get(0, email);
            return model;
        }

        // get the actual user object
        private static User Get(int userId = 0, string email = null)
        {
            User model = null;

            try
            {
                using (var dc = new ArmsContext())
                {
                    // User id if given
                    if (userId > 0)
                    {
                        model = dc.Users.FirstOrDefault(x => x.UserID == userId);
                    }
                    else if(email != null)
                    {
                        model = dc.Users.FirstOrDefault(x => x.Email == email);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }

        // returns whether there is a User with such email in the DB
        public static bool IsRegistered(string email)
        {
            bool isReg = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    isReg = !dc.Users.Any(u => u.Email == email);
                }
            }
            catch(Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return isReg;
        }

        // deletes a user with the specified userId
        public static void DeleteUser(int userId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var User = dc.Users.Where(u => u.UserID == userId).FirstOrDefault();
                    dc.Users.Remove(User);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
        }

        // get all students (users) for the specified course
        public static List<User> GetParticipantsForCourse(int courseId)
        {
            var users = new List<User>();
            try
            {
                using (var dc = new ArmsContext())
                {
                    users = dc.Participants.Where(ps => ps.CourseID == courseId && ps.ParticipantStatus == Participant.STATUS_ACTIVE).Select(ps => ps.User).ToList();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return users;
        }

        // get students (users) pending for a specified course
        public static List<User> GetPendingParticipantsForCourse(int courseId)
        {
            var list = new List<User>();
            try
            {
                using (var dc = new ArmsContext())
                {
                    list = dc.Participants.Where(ps => ps.CourseID == courseId && ps.ParticipantStatus == Participant.STATUS_PENDING).Select(ps => ps.User).ToList();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return list;
        }

        // for testing purposes
        public static List<User> GetAllUsers()
        {
            using (var dc = new ArmsContext())
            {
                var list = dc.Users.ToList();
                return list;
            }
        }
    }
}