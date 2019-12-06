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
    public static class ParticipantHelper
    {
        // get lecture by id
        public static Participant GetById(int participantId)
        {
            Participant model = null;

            try
            {
                using (var dc = new ArmsContext())
                {
                    model = dc.Participants.Include(x => x.User).Include(x => x.Course).FirstOrDefault(x => x.ParticipantID == participantId);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }

        // get participant attendance performance percentage
        public static int GetParticipantAttendance(int userId, int courseId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    // get number of lectures for the course
                    var lectures = dc.Lectures.Where(x => x.CourseID == courseId);
                    //int nol = lectures.Count();

                    // get number of attended lectures for the student
                    int noa = 0;
                    int nou = 0;
                    foreach(var lecture in lectures)
                    {
                        // check if student attended
                        if(dc.Attendees.Any(x => x.LectureID == lecture.LectureID && x.UserID == userId))
                            noa++;
                        else
                            nou++;
                    }
                    int all = noa + nou;
                    return (noa / all) * 100;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return -1;
        }

        // deletes a student with the targeted id
        public static void DeleteParticipant(int userId, int courseId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var student = dc.Participants.Where(u => u.UserID == userId && u.CourseID == courseId).FirstOrDefault();
                    dc.Participants.Remove(student);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
        }

        // for testing purposes
        public static List<Participant> GetAllParticipants()
        {
            using (var dc = new ArmsContext())
            {
                var list = dc.Participants.Include(x => x.User).Include(x => x.Course).ToList();
                return list;
            }
        }
    }
}