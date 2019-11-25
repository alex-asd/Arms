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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupervisorID { get; set; }

        public int UserID { get; set; }
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
        public bool Upsert()
        {
            bool success = false;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Supervisors.FirstOrDefault(x => x.SupervisorID == this.SupervisorID);

                    // Insert the new supervisor to the DB
                    if (sqlEntry == null)
                    {
                        dc.Supervisors.Add(this);
                    }

                    // Updates existing supervisor
                    if (sqlEntry != null)
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