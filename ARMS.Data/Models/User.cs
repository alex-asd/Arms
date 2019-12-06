using ARMS.Data.Helpers;
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
        [NotMapped]
        public static readonly string USER_STUDENT = "student";
        [NotMapped]
        public static readonly string USER_TEACHER = "teacher";

        public int UserID { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Course creator")]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string Type { get; set; }

        public User() { }

        public User(string firstName, string lastName, string email, string type)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Type = type;
        }
        
        #region Database Interactions
        
        public bool Upsert(BonusEnum.UpsertType upsertType = BonusEnum.UpsertType.Upsert)
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Users.FirstOrDefault(x => x.Email == this.Email);

                    if (sqlEntry == null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Insert))
                    {
                        // Insert the new user to the DB
                        dc.Users.Add(this);
                    }

                    if (sqlEntry != null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Update))
                    {
                        sqlEntry.FirstName = this.FirstName.ToLower();
                        sqlEntry.LastName = this.LastName;
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

        public bool Insert(BonusEnum.UpsertType upsertType = BonusEnum.UpsertType.Upsert)
        {
            return this.Upsert(BonusEnum.UpsertType.Insert);
        }

        public bool Update()
        {
            return this.Upsert(BonusEnum.UpsertType.Update);
        }

        public bool Delete()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Users.FirstOrDefault(x => x.UserID == this.UserID);
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
                    var sqlEntry = dc.Users.FirstOrDefault(x => x.Email == this.Email);
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

