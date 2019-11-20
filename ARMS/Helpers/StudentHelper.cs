using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using ARMS.Data.Models;
using ARMS.Data;

namespace ARMS.Helpers
{
    public static class StudentHelper
    {
        // get student by id
        public static Student GetById(int studentId)
        {
            var model = Get(studentId);
            return model;
        }

        // get student by username
        public static Student GetByUsername(string username)
        {
            var lowUsername = username.ToLower();
            var model = Get(0, lowUsername);
            return model;
        }

        // get the actual student object
        private static Student Get(int studentId = 0, string username = null)
        {
            Student model = null;

            try
            {
                using (var dc = new ArmsContext())
                {
                    // User id if given
                    if (studentId > 0)
                    {
                        model = dc.Students.Include(x => x.Courses).Include(x => x.Lectures).FirstOrDefault(x => x.ID == studentId);
                    }
                    else if (username != null)
                    {
                        model = dc.Students.Include(x => x.Courses).Include(x => x.Lectures).FirstOrDefault(x => x.Username == username);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return model;
        }

        // returns whether there is a student with such username in the DB
        public static bool IsRegistered(string username, string email)
        {
            var lowerUsername = username.ToLower();
            using (var dc = new ArmsContext())
            {
                return !dc.Students.Any(u => u.Username == lowerUsername);
            }
        }

        // deletes a student with the targeted username
        public static void DeleteStudent(string username)
        {
            try
            {
                using (var dc = new ArmsContext())
                {
                    var student = dc.Students.Where(u => u.Username == username.ToLower()).FirstOrDefault();
                    dc.Students.Remove(student);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var catchMsg = ex.Message;
            }
        }

        // for testing purposes
        public static List<Student> GetAllStudents()
        {
            using (var dc = new ArmsContext())
            {
                var list = dc.Students.Include(x => x.Courses).Include(x => x.Lectures).ToList();
                return list;
            }
        }
    }
}