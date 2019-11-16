namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ARMS.Data.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ARMS.Data.ArmsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ARMS.Data.ArmsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            context.Students.AddOrUpdate(x => x.ID,
                new Student() { ID = 1, FirstName = "Aleksandar", LastName = "Stoyanov", Email = "alex_d97@abv.bg", Password = "asddsaasd51" },
                new Student() { ID = 2, FirstName = "Johnny", LastName = "Pit", Email = "exmp1@exm.com", Password = "asddsaasd5" },
                new Student() { ID = 3, FirstName = "Bradd", LastName = "Depp", Email = "exmp2@exm.com", Password = "asddsaasd7" }
                );

            context.Teachers.AddOrUpdate(x => x.ID,
                new Teacher() { ID = 1, FirstName = "Alex", LastName = "Dinev", Email = "alex_d97@abv.bg", Password = "asddsaasd145" },
                new Teacher() { ID = 2, FirstName = "Kim", LastName = "Nielsen", Email = "exmp1@exm.com", Password = "asddsaasd2" },
                new Teacher() { ID = 3, FirstName = "Ole", LastName = "Google", Email = "exmp2@exm.com", Password = "asddsaasd3" }
                );

            context.Courses.AddOrUpdate(x => x.CourseID,
                new Course() { CourseID = 1, CourseName = "DNP-1", CourseDescription = ".NET for begginers"},
                new Course() { CourseID = 2, CourseName = "DNP-2", CourseDescription = "Advanced .NET" },
                new Course() { CourseID = 3, CourseName = "SJD-3", CourseDescription = "Hardcore Java" }
                );

            context.Lecutres.AddOrUpdate(x => x.CourseID,
                new Lecture() { LectureID = 1, Date = new DateTime(), CourseID = 1 },
                new Lecture() { LectureID = 2, Date = new DateTime(), CourseID = 1 },
                new Lecture() { LectureID = 3, Date = new DateTime(), CourseID = 3 }
                );
        }
    }
}
