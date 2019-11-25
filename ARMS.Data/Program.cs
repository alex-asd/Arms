using ARMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ARMS.Data.Helpers;

namespace ARMS.Data
{
    class Program
    {
        static public void Main(String[] args)
        {
            //using (var dc = new ArmsContext())
            //{
            //    try
            //    {
            //        var u1 = new User("Kim JR", "Nielsen", "dotnetMaster@dot.net", "theDotNetGuru", "teacher");
            //        var u2 = new User("Kasper JR", "Mortensen", "iprsJR@dot.net", "TryHardJR", "student");
            //        var u3 = new User("Jacob JR", "Odinsson", "uslsJR@dot.net", "Teddorektx", "teacher");
            //        var u4 = new User("Hristo JR", "Kosata", "nqmamkosmiJR@dot.net", "101PricheskiJR", "student");
            //        var u5 = new User("Alex JR", "Stoyanov", "asd20@dot.net", "dotNetNoobJR", "student");

            //        var c1 = new Course("DNP1-AY-Y19", "dotNet programming for noobs", u1.GetUserID());
            //        var c2 = new Course("DNP2-AX-Y19", "dotNet programming for intermediates", u1.GetUserID());
            //        var c3 = new Course("SDJ3-AX-Y19", "Detributed systems", u3.GetUserID());

            //        u1.Upsert();
            //        u2.Upsert();
            //        u3.Upsert();
            //        u4.Upsert();
            //        u5.Upsert();

            //        c1.Upsert();
            //        c2.Upsert();
            //        c3.Upsert();

            //        var p1 = new Participant(u2.GetUserID(), c1.GetCourseID(), "Active");
            //        var p2 = new Participant(u4.GetUserID(), c2.GetCourseID(), "Active");
            //        var p3 = new Participant(u5.GetUserID(), c1.GetCourseID(), "Active");

            //        p1.Upsert();
            //        p2.Upsert();
            //        p3.Upsert();


            //        var l1 = new Lecture(new DateTime(2019, 11, 23, 8, 20, 0), new DateTime(2019, 11, 23, 12, 0, 0), c1.GetCourseID());
            //        var l2 = new Lecture(new DateTime(2019, 11, 23, 8, 20, 0), new DateTime(2019, 11, 23, 12, 0, 0), c2.GetCourseID());
            //        var l3 = new Lecture(new DateTime(2019, 11, 23, 8, 20, 0), new DateTime(2019, 11, 23, 12, 0, 0), c3.GetCourseID());

            //        l1.Upsert();
            //        l2.Upsert();
            //        l3.Upsert();

            //        var a1 = new Attendee(u2.GetUserID(), l1.GetLectureID(), "1sdasd561ssa");
            //        var a2 = new Attendee(u4.GetUserID(), l2.GetLectureID(), "1sfbcxd561ysa34");
            //        var a3 = new Attendee(u5.GetUserID(), l1.GetLectureID(), "1sdasd561asdsa");

            //        a1.Upsert();
            //        a2.Upsert();
            //        a3.Upsert();

            //        var s1 = new Supervisor(u1.GetUserID(), c1.GetCourseID());
            //        var s2 = new Supervisor(u1.GetUserID(), c2.GetCourseID());
            //        var s3 = new Supervisor(u2.GetUserID(), c3.GetCourseID());

            //        s1.Upsert();
            //        s2.Upsert();
            //        s3.Upsert();
                    
            //        dc.SaveChanges();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.InnerException);
            //    }
            //}
            //while (true) ;

            // HELPERS TEST
            //try
            //{
            //    using (var dc = new ArmsContext())
            //    {
            //        var res1 = CourseHelper.GetByName("DNP1-AY-Y19");

            //        var res2 = LectureHelper.GetAllLectures();

            //        var res3 = StudentHelper.GetById(56);
            //        var res4 = StudentHelper.GetAllStudents();
            //        var res5 = StudentHelper.GetByUsername("101PricheskiJR");

            //        var res6 = TeacherHelper.GetAllTeachers();
            //    }
            //}
            //catch(Exception ex)
            //{
            //    var catchMsg = ex.Message;
            //}

        }
    }
}
