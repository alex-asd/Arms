namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lecturemodelupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeacherLectures", "Teacher_ID", "dbo.Teachers");
            DropForeignKey("dbo.TeacherLectures", "Lecture_LectureID", "dbo.Lectures");
            DropIndex("dbo.TeacherLectures", new[] { "Teacher_ID" });
            DropIndex("dbo.TeacherLectures", new[] { "Lecture_LectureID" });
            AddColumn("dbo.Lectures", "From", c => c.DateTime(nullable: false));
            AddColumn("dbo.Lectures", "To", c => c.DateTime(nullable: false));
            AddColumn("dbo.Lectures", "CheckInEnabled", c => c.Boolean(nullable: false));
            DropColumn("dbo.Lectures", "Date");
            DropTable("dbo.TeacherLectures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TeacherLectures",
                c => new
                    {
                        Teacher_ID = c.Int(nullable: false),
                        Lecture_LectureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_ID, t.Lecture_LectureID });
            
            AddColumn("dbo.Lectures", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Lectures", "CheckInEnabled");
            DropColumn("dbo.Lectures", "To");
            DropColumn("dbo.Lectures", "From");
            CreateIndex("dbo.TeacherLectures", "Lecture_LectureID");
            CreateIndex("dbo.TeacherLectures", "Teacher_ID");
            AddForeignKey("dbo.TeacherLectures", "Lecture_LectureID", "dbo.Lectures", "LectureID", cascadeDelete: true);
            AddForeignKey("dbo.TeacherLectures", "Teacher_ID", "dbo.Teachers", "ID", cascadeDelete: true);
        }
    }
}
