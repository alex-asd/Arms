using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMS.Data.Models
{
    public class Attendee
    {
        public string BluetoothAddress { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendeeID { get; set; }


        public int UserID { get; set; }     
        public int LectureID { get; set; }


        public virtual User User { get; set; }
        public virtual Lecture Lecture { get; set; }

        public Attendee() { }

        public Attendee(int userId, int lectureId, string btha)
        {
            this.UserID = userId;
            this.LectureID = lectureId;
            this.BluetoothAddress = btha;
        }

        #region Database Interactions
        public bool Upsert()
        {
            bool success = false;

            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Attendees.FirstOrDefault(x => x.AttendeeID == this.AttendeeID);

                    // Insert new attendee to DB
                    if (sqlEntry == null)
                    {
                        dc.Attendees.Add(this);
                    }

                    // Update existing entry
                    if (sqlEntry != null)
                    {
                        sqlEntry.LectureID = this.LectureID;
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
                    var sqlEntry = dc.Attendees.FirstOrDefault(x => x.AttendeeID == this.AttendeeID);
                    dc.Attendees.Remove(sqlEntry);

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
