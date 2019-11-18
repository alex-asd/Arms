using ARMS.Data;
using ARMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ARMS.Controllers
{
    public class UtilController : ApiController
    {
        //GET api/Util
        public Student GetStudentWithID()
        {
            var student = new Student() { FirstName = "Aleksandar", LastName = "Stoyanov", Email = "asd@dsa.bg", Password = "somehash" };
            return student;
        }
    }
}
