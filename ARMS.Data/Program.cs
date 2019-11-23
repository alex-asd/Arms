using ARMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARMS.Data.Helpers;

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
                    //var c1 = new Course("DNP1-AY-Y19", "dotNet programming for noobs");
                    //var c2 = new Course("DNP2-AX-Y19", "dotNet programming for intermediates");
                    //var c3 = new Course("SDJ3-AX-Y19", "Detributed systems");

                    //var t1 = new Teacher("Kim", "Nielsen", "dotnetMaster@dot.net", "theDotNetGuru");
                    //var t2 = new Teacher("Kasper", "Mortensen", "iprs@dot.net", "TryHard");
                    //var t3 = new Teacher("Jacob", "Odinsson", "usls@dot.net", "Teddorektx");
                    //var t4 = new Teacher("Hristo", "Kosata", "nqmamkosmi@dot.net", "101Pricheski");
                    //var t5 = new Teacher("Alex", "Stoyanov", "asd97@dot.net", "dotNetNoob");

                    //var s1 = new Student("Kim JR", "Nielsen", "dotnetMasterJR@dot.net", "theDotNetGuruJR");
                    //var s2 = new Student("Kasper JR", "Mortensen", "iprsJR@dot.net", "TryHardJR");
                    //var s3 = new Student("Jacob JR", "Odinsson", "uslsJR@dot.net", "TeddorektxJR");
                    //var s4 = new Student("Hristo JR", "Kosata", "nqmamkosmiJR@dot.net", "101PricheskiJR");
                    //var s5 = new Student("Alex JR", "Stoyanov", "asd20@dot.net", "dotNetNoobJR");

                    //c1.Upsert();
                    //c2.Upsert();
                    //c3.Upsert();
                    //s1.Upsert();
                    //s2.Upsert();
                    //s3.Upsert();
                    //s4.Upsert();
                    //s5.Upsert();
                    //t1.Upsert();
                    //t2.Upsert();
                    //t3.Upsert();
                    //t4.Upsert();
                    //t5.Upsert();

                    //var l1 = new Lecture(new DateTime(2019, 11, 23, 8, 20, 0), new DateTime(2019, 11, 23, 12, 0, 0), 56);
                    //var l2 = new Lecture(new DateTime(2019, 11, 23, 8, 20, 0), new DateTime(2019, 11, 23, 12, 0, 0), 57);
                    
                    //l1.Upsert(56);
                    //l2.Upsert(57);

                    //c1.Students = new List<Student> { s1, s2, s3 };
                    //c1.Teachers = new List<Teacher> { t1, t2 };
                    //t1.Courses = new List<Course> { c1 };
                    //t2.Courses = new List<Course> { c1 };
                    //c1.Lectures = new List<Lecture> { l1 };

                    //c2.Students = new List<Student> { s4, s2, s5 };
                    //c2.Teachers = new List<Teacher> { t3 };
                    //t3.Courses = new List<Course> { c2 };
                    //c2.Lectures = new List<Lecture> { l2 };

                    //c3.Students = new List<Student> { s1, s5 };
                    //c3.Teachers = new List<Teacher> { t4, t5 };
                    //t4.Courses = new List<Course> { c3 };
                    //t5.Courses = new List<Course> { c3 };


                    //s1.Courses = new List<Course> { c1, c3 };
                    //s2.Courses = new List<Course> { c1, c2 };
                    //s3.Courses = new List<Course> { c1 };
                    //s4.Courses = new List<Course> { c2 };
                    //s5.Courses = new List<Course> { c2, c3 };

                    //l1.Students = new List<Student> { s1 };
                    //l2.Students = new List<Student> { s2, s4 };




                    //c1.Upsert();
                    //c2.Upsert();
                    //c3.Upsert();
                    //s1.Upsert();
                    //s2.Upsert();
                    //s3.Upsert();
                    //s4.Upsert();
                    //s5.Upsert();
                    //t1.Upsert();
                    //t2.Upsert();
                    //t3.Upsert();
                    //t4.Upsert();
                    //t5.Upsert();
                    //l1.Upsert(56);
                    //l2.Upsert(57);

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
