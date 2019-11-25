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
        // get User by id
        public static User GetById(int userId)
        {
            var model = Get(userId);
            return model;
        }

        // get User by username
        public static User GetByUsername(string username)
        {
            var lowUsername = username.ToLower();
            var model = Get(0, lowUsername);
            return model;
        }

        // get the actual User object
        private static User Get(int userId = 0, string username = null)
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
                    else if (username != null)
                    {
                        model = dc.Users.FirstOrDefault(x => x.Username == username);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }

        // returns whether there is a User with such username in the DB
        public static bool IsRegistered(string username, string email)
        {
            var lowerUsername = username.ToLower();
            using (var dc = new ArmsContext())
            {
                return !dc.Users.Any(u => u.Username == lowerUsername);
            }
        }

        // deletes a User with the targeted username
        public static void DeleteUser(string username)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var User = dc.Users.Where(u => u.Username == username.ToLower()).FirstOrDefault();
                    dc.Users.Remove(User);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
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