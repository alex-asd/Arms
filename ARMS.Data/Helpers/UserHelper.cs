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
      
        public static User GetByEmail(string email)
        {
            var model = Get(0, email);
            return model;
        }

        // get the actual User object
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
            using (var dc = new ArmsContext())
            {
                return !dc.Users.Any(u => u.Email == email);
            }
        }

        // deletes a User with the targeted username
        public static void DeleteUser(string email)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var User = dc.Users.Where(u => u.Email == email).FirstOrDefault();
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