using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class Student : User
    {
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }

        public Student()
        {
            this.Courses = new HashSet<Course>();
            this.Lectures = new HashSet<Lecture>();
        }

        public Student(string firstName, string lastName, string email, string password) : base(firstName, lastName, email, password)
        {

        }
    }
}
