namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelsredesigned : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentCourses", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.StudentCourses", "Course_CourseID", "dbo.Courses");
            DropForeignKey("dbo.StudentLectures", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.StudentLectures", "Lecture_LectureID", "dbo.Lectures");
            DropForeignKey("dbo.TeacherCourses", "Teacher_ID", "dbo.Teachers");
            DropForeignKey("dbo.TeacherCourses", "Course_CourseID", "dbo.Courses");
            DropIndex("dbo.Students", new[] { "Username" });
            DropIndex("dbo.Students", new[] { "Email" });
            DropIndex("dbo.Teachers", new[] { "Username" });
            DropIndex("dbo.Teachers", new[] { "Email" });
            DropIndex("dbo.StudentCourses", new[] { "Student_ID" });
            DropIndex("dbo.StudentCourses", new[] { "Course_CourseID" });
            DropIndex("dbo.StudentLectures", new[] { "Student_ID" });
            DropIndex("dbo.StudentLectures", new[] { "Lecture_LectureID" });
            DropIndex("dbo.TeacherCourses", new[] { "Teacher_ID" });
            DropIndex("dbo.TeacherCourses", new[] { "Course_CourseID" });
            CreateTable(
                "dbo.Attendees",
                c => new
                    {
                        AttendeeID = c.Int(nullable: false, identity: true),
                        BluetoothAddress = c.String(),
                        UserID = c.Int(nullable: false),
                        LectureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AttendeeID)
                .ForeignKey("dbo.Lectures", t => t.LectureID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.LectureID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Participants",
                c => new
                    {
                        ParticipantID = c.Int(nullable: false, identity: true),
                        ParticipantStatus = c.String(),
                        UserID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParticipantID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.Supervisors",
                c => new
                    {
                        SupervisorID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SupervisorID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.CourseID);
            
            AddColumn("dbo.Courses", "CreatorID", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "CreatorID");
            AddForeignKey("dbo.Courses", "CreatorID", "dbo.Users", "UserID", cascadeDelete: true);
            DropTable("dbo.Students");
            DropTable("dbo.Teachers");
            DropTable("dbo.StudentCourses");
            DropTable("dbo.StudentLectures");
            DropTable("dbo.TeacherCourses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TeacherCourses",
                c => new
                    {
                        Teacher_ID = c.Int(nullable: false),
                        Course_CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_ID, t.Course_CourseID });
            
            CreateTable(
                "dbo.StudentLectures",
                c => new
                    {
                        Student_ID = c.Int(nullable: false),
                        Lecture_LectureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_ID, t.Lecture_LectureID });
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        Student_ID = c.Int(nullable: false),
                        Course_CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_ID, t.Course_CourseID });
            
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
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.Supervisors", "UserID", "dbo.Users");
            DropForeignKey("dbo.Supervisors", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Participants", "UserID", "dbo.Users");
            DropForeignKey("dbo.Participants", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Attendees", "UserID", "dbo.Users");
            DropForeignKey("dbo.Attendees", "LectureID", "dbo.Lectures");
            DropForeignKey("dbo.Courses", "CreatorID", "dbo.Users");
            DropIndex("dbo.Supervisors", new[] { "CourseID" });
            DropIndex("dbo.Supervisors", new[] { "UserID" });
            DropIndex("dbo.Participants", new[] { "CourseID" });
            DropIndex("dbo.Participants", new[] { "UserID" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Courses", new[] { "CreatorID" });
            DropIndex("dbo.Attendees", new[] { "LectureID" });
            DropIndex("dbo.Attendees", new[] { "UserID" });
            DropColumn("dbo.Courses", "CreatorID");
            DropTable("dbo.Supervisors");
            DropTable("dbo.Participants");
            DropTable("dbo.Users");
            DropTable("dbo.Attendees");
            CreateIndex("dbo.TeacherCourses", "Course_CourseID");
            CreateIndex("dbo.TeacherCourses", "Teacher_ID");
            CreateIndex("dbo.StudentLectures", "Lecture_LectureID");
            CreateIndex("dbo.StudentLectures", "Student_ID");
            CreateIndex("dbo.StudentCourses", "Course_CourseID");
            CreateIndex("dbo.StudentCourses", "Student_ID");
            CreateIndex("dbo.Teachers", "Email", unique: true);
            CreateIndex("dbo.Teachers", "Username", unique: true);
            CreateIndex("dbo.Students", "Email", unique: true);
            CreateIndex("dbo.Students", "Username", unique: true);
            AddForeignKey("dbo.TeacherCourses", "Course_CourseID", "dbo.Courses", "CourseID", cascadeDelete: true);
            AddForeignKey("dbo.TeacherCourses", "Teacher_ID", "dbo.Teachers", "ID", cascadeDelete: true);
            AddForeignKey("dbo.StudentLectures", "Lecture_LectureID", "dbo.Lectures", "LectureID", cascadeDelete: true);
            AddForeignKey("dbo.StudentLectures", "Student_ID", "dbo.Students", "ID", cascadeDelete: true);
            AddForeignKey("dbo.StudentCourses", "Course_CourseID", "dbo.Courses", "CourseID", cascadeDelete: true);
            AddForeignKey("dbo.StudentCourses", "Student_ID", "dbo.Students", "ID", cascadeDelete: true);
        }
    }
}
