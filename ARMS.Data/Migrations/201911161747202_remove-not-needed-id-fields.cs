namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removenotneededidfields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Students", "StudentID");
            DropColumn("dbo.Teachers", "TeacherID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teachers", "TeacherID", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "StudentID", c => c.Int(nullable: false));
        }
    }
}
