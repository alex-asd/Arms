namespace ARMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TypeOfUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TypeOfUser");
        }
    }
}
