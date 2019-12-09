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
    public static class LectureHelper
    {
        // get lecture by id
        public static Lecture GetById(int LectureId)
        {
            Lecture model = null;

            try
            {
                using (var dc = new ArmsContext())
                {
                    model = dc.Lectures.Include(x => x.Course).FirstOrDefault(x => x.LectureID == LectureId);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }

        // deletes a lecture with the targeted id
        public static void DeleteLecture(int lectureId)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var lecture = dc.Lectures.Where(u => u.LectureID == lectureId).FirstOrDefault();
                    dc.Lectures.Remove(lecture);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
        }

        // for testing purposes
        public static List<Lecture> GetAllLectures()
        {
            using (var dc = new ArmsContext())
            {
                var list = dc.Lectures.Include(x => x.Course).ToList();
                return list;
            }
        }

        // get lectures for a course
        public static List<Lecture> GetLecturesForCourse(int courseId)
        {
            var list = new List<Lecture>();

            try
            {
                using (var dc = new ArmsContext())
                {
                    list = dc.Lectures.Where(l => l.CourseID == courseId).Include(x => x.Course).ToList();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }

            return list;
        }
    }
}