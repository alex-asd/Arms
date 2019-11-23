namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatemodels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Lectures", "CourseID", "dbo.Courses");
            DropIndex("dbo.Lectures", new[] { "CourseID" });
            AlterColumn("dbo.Lectures", "CourseID", c => c.Int(nullable: false));
            CreateIndex("dbo.Lectures", "CourseID");
            AddForeignKey("dbo.Lectures", "CourseID", "dbo.Courses", "CourseID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lectures", "CourseID", "dbo.Courses");
            DropIndex("dbo.Lectures", new[] { "CourseID" });
            AlterColumn("dbo.Lectures", "CourseID", c => c.Int());
            CreateIndex("dbo.Lectures", "CourseID");
            AddForeignKey("dbo.Lectures", "CourseID", "dbo.Courses", "CourseID");
        }
    }
}
