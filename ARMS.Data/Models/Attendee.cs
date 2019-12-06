using ARMS.Data.Helpers;
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
        [Display(Name = "Bluetooth Address")]
        public string BluetoothAddress { get; set; }
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendeeID { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }
        [Key, Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        public bool Upsert(BonusEnum.UpsertType upsertType = BonusEnum.UpsertType.Upsert)
        {
            bool success = false;

            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Attendees.FirstOrDefault(x => x.AttendeeID == this.AttendeeID);

                    // Insert new attendee to DB
                    if (sqlEntry == null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Insert))
                    {
                        dc.Attendees.Add(this);
                    }

                    // Update existing entry
                    if (sqlEntry != null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Update))
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
