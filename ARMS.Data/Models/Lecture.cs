using ARMS.Data.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class Lecture
    {
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm:ss}")]
        public DateTime From { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm:ss}")]
        public DateTime To { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LectureID { get; set; }
        [Display(Name = "Check-in")]
        public bool CheckInEnabled { get; set; }
        
        [Required]
        public int CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }
                
        public Lecture() { }

        public Lecture(DateTime from, DateTime to, int courseID)
        {
            this.From = from;
            this.To = to;
            this.CourseID = courseID;
            CheckInEnabled = false;
        }

        #region Database Interactions
        public bool Upsert(BonusEnum.UpsertType upsertType = BonusEnum.UpsertType.Upsert)
        {
            bool success = false;

            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Lectures.FirstOrDefault(x => x.LectureID == this.LectureID);

                    // Insert new lecture to DB
                    if (sqlEntry == null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Insert))
                    {
                        dc.Lectures.Add(this);
                    }

                    // Update existing entry
                    if (sqlEntry != null && (upsertType == BonusEnum.UpsertType.Upsert || upsertType == BonusEnum.UpsertType.Update))
                    {
                        sqlEntry.From = this.From;
                        sqlEntry.To = this.To;
                        sqlEntry.CheckInEnabled = this.CheckInEnabled;
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
                    var sqlEntry = dc.Lectures.FirstOrDefault(x => x.LectureID == this.LectureID);
                    dc.Lectures.Remove(sqlEntry);

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

        public int GetLectureID()
        {
            int result = 0;
            try
            {
                using (var dc = new ArmsContext())
                {
                    var sqlEntry = dc.Lectures.FirstOrDefault(x => x.LectureID == this.LectureID);
                    result = sqlEntry.LectureID;

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