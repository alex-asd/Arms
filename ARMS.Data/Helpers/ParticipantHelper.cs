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
        public static decimal GetParticipantAttendance(int userId, int courseId)
        {
            decimal result = -1;
            try
            {
                using (var dc = new ArmsContext())
                {
                    // get number of lectures for the course that have happened
                    var lectures = dc.Lectures.Where(x => x.CourseID == courseId && x.To <= DateTime.Now).ToList();
                    decimal numOfLectures = lectures.Count();

                    // get number of attended lectures for the student
                    decimal noa = 0m;

                    foreach(var lecture in lectures)
                    {
                        // check if student attended
                        if(dc.Attendees.Any(x => x.LectureID == lecture.LectureID && x.UserID == userId))
                            noa++;
                    }

                    if (numOfLectures == 0)
                        return 0;
                    
                    result = (noa / numOfLectures) * 100m;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return result;
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

        // get all participants for course
        public static List<Participant> GetParticipantsForCourse(int courseId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var list = dc.Participants.Where(x => x.CourseID == courseId).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return null;
        }

        // get the number of pending participants for course
        public static int GetCountOfPendingParticipants(int courseId)
        {
            int num = 0;
            try
            {
                using (var dc = new ArmsContext())
                {
                    num = dc.Participants.Where(ps => ps.CourseID == courseId && ps.ParticipantStatus == Participant.STATUS_PENDING).Select(ps => ps.User).Count();

                    return num;
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
            return num;
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