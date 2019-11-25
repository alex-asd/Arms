using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class User
    {
        public int UserID { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        public string Type { get; set; }

        public User() { }

        public User(string firstName, string lastName, string email, string username, string type)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Username = username.ToLower();
            this.Type = type;
        }

        #region Database Interactions
        public bool Upsert()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Users.FirstOrDefault(x => x.Username == this.Username.ToLower());

                    if (sqlEntry == null)
                    {
                        // Insert the new user to the DB
                        this.Username = Username.ToLower();
                        dc.Users.Add(this);
                    }

                    if (sqlEntry != null)
                    {
                        sqlEntry.FirstName = this.FirstName.ToLower();
                        sqlEntry.LastName = this.LastName;
                        sqlEntry.Username = this.Username;
                        sqlEntry.Type = this.Type;
                        sqlEntry.Email = this.Email;
                    }

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

        public bool Delete()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Users.FirstOrDefault(x => x.Username == this.Username);
                    dc.Users.Remove(sqlEntry);

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

        public int GetUserID()
        {
            int result = 0;

            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Users.FirstOrDefault(x => x.Username == this.Username);
                    result = sqlEntry.UserID;

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return result;
        }
        #endregion
    }
}

