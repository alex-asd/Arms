namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelsupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Username", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Teachers", "Username", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Courses", "CourseName", c => c.String(nullable: false, maxLength: 300));
            CreateIndex("dbo.Courses", "CourseName", unique: true);
            CreateIndex("dbo.Students", "Username", unique: true);
            CreateIndex("dbo.Students", "Email", unique: true);
            CreateIndex("dbo.Teachers", "Username", unique: true);
            CreateIndex("dbo.Teachers", "Email", unique: true);
            DropColumn("dbo.Students", "Password");
            DropColumn("dbo.Teachers", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teachers", "Password", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Students", "Password", c => c.String(nullable: false, maxLength: 20));
            DropIndex("dbo.Teachers", new[] { "Email" });
            DropIndex("dbo.Teachers", new[] { "Username" });
            DropIndex("dbo.Students", new[] { "Email" });
            DropIndex("dbo.Students", new[] { "Username" });
            DropIndex("dbo.Courses", new[] { "CourseName" });
            AlterColumn("dbo.Courses", "CourseName", c => c.String(nullable: false));
            DropColumn("dbo.Teachers", "Username");
            DropColumn("dbo.Students", "Username");
        }
    }
}
