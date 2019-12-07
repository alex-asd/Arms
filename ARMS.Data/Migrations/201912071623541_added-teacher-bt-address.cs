namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedteacherbtaddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lectures", "CheckInBluetoothAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Lectures", "CheckInBluetoothAddress");
        }
    }
}
