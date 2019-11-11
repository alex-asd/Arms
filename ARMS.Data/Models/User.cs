using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        public User() { }

        public User(string firstName, string lastName, string email, string password)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;
        }

        //#region Database Interactions

        //public bool Upsert(BonusEnum.UpsertType upsertType = BonusEnum.UpsertType.Upsert)
        //{
        //    bool success = false;
        //    try
        //    {
        //        using (var dc = new CockpitContext())
        //        {
        //            var sqlEntry = dc.User.FirstOrDefault(x => x.UserId == this.UserId);
        //            // Insert the new user to the DB
        //            if (sqlEntry == null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Insert))
        //            {
        //                dc.User.Add(this);
        //            }

        //            //  Update existing user
        //            if (sqlEntry != null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Update))
        //            {
        //                sqlEntry.Username = this.Username;
        //                //sqlEntry.PermissionId = this.PermissionId;
        //                sqlEntry.Inactive = this.Inactive;
        //                //sqlEntry.UserId = this.UserId;
        //                sqlEntry.Permission.Admin = this.Permission.Admin;
        //                sqlEntry.Permission.VMEdit = this.Permission.VMEdit;
        //                sqlEntry.Permission.VMCreate = this.Permission.VMCreate;
        //            }

        //            dc.SaveChanges();
        //        }
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var catchMsg = ex.Message;
        //    }
        //    return success;
        //}

        //public bool Update()
        //{
        //    return this.Upsert(BonusEnum.UpsertType.Update);
        //}

        //public bool Insert()
        //{
        //    return this.Upsert(BonusEnum.UpsertType.Insert);
        //}

        //public bool Delete()
        //{
        //    bool success = false;
        //    try
        //    {
        //        using (var dc = new CockpitContext())
        //        {
        //            var sqlEntry = dc.User.FirstOrDefault(x => x.UserId == this.UserId);
        //            dc.User.Remove(sqlEntry);

        //            dc.SaveChanges();
        //        }
        //        success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var catchMsg = ex.Message;
        //    }
        //    return success;
        //}

        //#endregion
    }
}

