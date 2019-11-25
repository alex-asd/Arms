using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMS.Data.Models
{
    public class Participant
    {
        public string ParticipantStatus { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantID { get; set; }

        public int UserID { get; set; }
        public int CourseID { get; set; }
        
        public virtual User User { get; set; }
        public virtual Course Course { get; set; }

        public Participant() { }

        public Participant(int userId, int courseId, string participantStatus)
        {
            this.UserID = userId;
            this.CourseID = courseId;
            this.ParticipantStatus = participantStatus;
        }

        #region Database Interactions
        public bool Upsert()
        {
            bool success = false;

            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Participants.FirstOrDefault(x => x.ParticipantID == this.ParticipantID);

                    // Insert new lecture to DB
                    if (sqlEntry == null)
                    {
                        // Insert the new lecture to the db
                        dc.Participants.Add(this);
                    }

                    // Update existing entry
                    if (sqlEntry != null)
                    {
                        sqlEntry.CourseID = this.CourseID;
                        sqlEntry.UserID = this.UserID;
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
                    var sqlEntry = dc.Participants.FirstOrDefault(x => x.ParticipantID == this.ParticipantID);
                    dc.Participants.Remove(sqlEntry);

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
