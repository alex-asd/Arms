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
        private static Lecture GetById(int LectureId)
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

        // for testing purposes
        public static List<Lecture> GetAllLectures()
        {
            using (var dc = new ArmsContext())
            {
                var list = dc.Lectures.Include(x => x.Course).ToList();
                return list;
            }
        }
    }
}