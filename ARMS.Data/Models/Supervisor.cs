using ARMS.Data.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class Supervisor
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupervisorID { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }
        [Key, Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseID { get; set; }

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }

        public Supervisor() { }
        public Supervisor(int userId, int courseId)
        {
            this.UserID = userId;
            this.CourseID = courseId;
        }

        #region Database Interactions
        public bool Upsert(BonusEnum.UpsertType upsertType = BonusEnum.UpsertType.Upsert)
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Supervisors.FirstOrDefault(x => x.SupervisorID == this.SupervisorID);

                    // Insert the new supervisor to the DB
                    if (sqlEntry == null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Insert))
                    {
                        dc.Supervisors.Add(this);
                    }

                    // Updates existing supervisor
                    if (sqlEntry != null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Update))
                    {
                        sqlEntry.UserID = this.UserID;
                        sqlEntry.CourseID = this.CourseID;
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
                    var sqlEntry = dc.Supervisors.FirstOrDefault(x => x.SupervisorID == this.SupervisorID);
                    dc.Supervisors.Remove(sqlEntry);

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
        #endregion
    }
}