namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeusernamefromuser : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "Username" });
            DropColumn("dbo.Users", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Users", "Username", unique: true);
        }
    }
}
