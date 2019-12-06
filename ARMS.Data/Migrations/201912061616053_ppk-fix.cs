namespace ARMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ppkfix : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Attendees");
            DropPrimaryKey("dbo.Participants");
            DropPrimaryKey("dbo.Supervisors");
            AddPrimaryKey("dbo.Attendees", new[] { "AttendeeID", "UserID", "LectureID" });
            AddPrimaryKey("dbo.Participants", new[] { "ParticipantID", "UserID", "CourseID" });
            AddPrimaryKey("dbo.Supervisors", new[] { "SupervisorID", "UserID", "CourseID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Supervisors");
            DropPrimaryKey("dbo.Participants");
            DropPrimaryKey("dbo.Attendees");
            AddPrimaryKey("dbo.Supervisors", "SupervisorID");
            AddPrimaryKey("dbo.Participants", "ParticipantID");
            AddPrimaryKey("dbo.Attendees", "AttendeeID");
        }
    }
}
