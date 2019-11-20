using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ARMS.Data.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Email { get; set; }

        public User() { }

        public User(string firstName, string lastName, string email, string username)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Username = username;
        }
    }
}

