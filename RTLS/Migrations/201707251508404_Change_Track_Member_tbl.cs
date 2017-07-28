namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Track_Member_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrackMembers", "PostDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.TrackMembers", "RecieveDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrackMembers", "RecieveDateTime");
            DropColumn("dbo.TrackMembers", "PostDateTime");
        }
    }
}
