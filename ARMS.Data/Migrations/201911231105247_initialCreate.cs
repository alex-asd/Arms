namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseID = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false, maxLength: 300),
                        CourseDescription = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.CourseID)
                .Index(t => t.CourseName, unique: true);
            
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        LectureID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LectureID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        Student_ID = c.Int(nullable: false),
                        Course_CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_ID, t.Course_CourseID })
                .ForeignKey("dbo.Students", t => t.Student_ID, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_CourseID, cascadeDelete: true)
                .Index(t => t.Student_ID)
                .Index(t => t.Course_CourseID);
            
            CreateTable(
                "dbo.StudentLectures",
                c => new
                    {
                        Student_ID = c.Int(nullable: false),
                        Lecture_LectureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_ID, t.Lecture_LectureID })
                .ForeignKey("dbo.Students", t => t.Student_ID, cascadeDelete: true)
                .ForeignKey("dbo.Lectures", t => t.Lecture_LectureID, cascadeDelete: true)
                .Index(t => t.Student_ID)
                .Index(t => t.Lecture_LectureID);
            
            CreateTable(
                "dbo.TeacherCourses",
                c => new
                    {
                        Teacher_ID = c.Int(nullable: false),
                        Course_CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_ID, t.Course_CourseID })
                .ForeignKey("dbo.Teachers", t => t.Teacher_ID, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_CourseID, cascadeDelete: true)
                .Index(t => t.Teacher_ID)
                .Index(t => t.Course_CourseID);
            
            CreateTable(
                "dbo.TeacherLectures",
                c => new
                    {
                        Teacher_ID = c.Int(nullable: false),
                        Lecture_LectureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_ID, t.Lecture_LectureID })
                .ForeignKey("dbo.Teachers", t => t.Teacher_ID, cascadeDelete: true)
                .ForeignKey("dbo.Lectures", t => t.Lecture_LectureID, cascadeDelete: true)
                .Index(t => t.Teacher_ID)
                .Index(t => t.Lecture_LectureID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeacherLectures", "Lecture_LectureID", "dbo.Lectures");
            DropForeignKey("dbo.TeacherLectures", "Teacher_ID", "dbo.Teachers");
            DropForeignKey("dbo.TeacherCourses", "Course_CourseID", "dbo.Courses");
            DropForeignKey("dbo.TeacherCourses", "Teacher_ID", "dbo.Teachers");
            DropForeignKey("dbo.StudentLectures", "Lecture_LectureID", "dbo.Lectures");
            DropForeignKey("dbo.StudentLectures", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.StudentCourses", "Course_CourseID", "dbo.Courses");
            DropForeignKey("dbo.StudentCourses", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.Lectures", "CourseID", "dbo.Courses");
            DropIndex("dbo.TeacherLectures", new[] { "Lecture_LectureID" });
            DropIndex("dbo.TeacherLectures", new[] { "Teacher_ID" });
            DropIndex("dbo.TeacherCourses", new[] { "Course_CourseID" });
            DropIndex("dbo.TeacherCourses", new[] { "Teacher_ID" });
            DropIndex("dbo.StudentLectures", new[] { "Lecture_LectureID" });
            DropIndex("dbo.StudentLectures", new[] { "Student_ID" });
            DropIndex("dbo.StudentCourses", new[] { "Course_CourseID" });
            DropIndex("dbo.StudentCourses", new[] { "Student_ID" });
            DropIndex("dbo.Teachers", new[] { "Email" });
            DropIndex("dbo.Teachers", new[] { "Username" });
            DropIndex("dbo.Students", new[] { "Email" });
            DropIndex("dbo.Students", new[] { "Username" });
            DropIndex("dbo.Lectures", new[] { "CourseID" });
            DropIndex("dbo.Courses", new[] { "CourseName" });
            DropTable("dbo.TeacherLectures");
            DropTable("dbo.TeacherCourses");
            DropTable("dbo.StudentLectures");
            DropTable("dbo.StudentCourses");
            DropTable("dbo.Teachers");
            DropTable("dbo.Students");
            DropTable("dbo.Lectures");
            DropTable("dbo.Courses");
        }
    }
}
