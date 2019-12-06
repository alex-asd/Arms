using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using ARMS.Data.Models;

namespace ARMS.Data.Helpers
{
    public static class AttendeeHelper
    {
        // get Attendee by id
        public static Attendee GetById(int attendeeId)
        {
            Attendee model = null;

            try
            {
                using (var dc = new ArmsContext())
                {
                    model = dc.Attendees.Include(x => x.User).Include(x => x.Lecture).FirstOrDefault(x => x.AttendeeID == attendeeId);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }

        // deletes a attendee with the targeted id
        public static void DeleteAttendee(int userId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var att = dc.Attendees.Where(u => u.UserID == userId).FirstOrDefault();
                    dc.Attendees.Remove(att);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
        }

        // for testing purposes
        public static List<Attendee> GetAllLectures()
        {
            using (var dc = new ArmsContext())
            {
                var list = dc.Attendees.Include(x => x.User).Include(x => x.Lecture).ToList();
                return list;
            }
        }
    }
}