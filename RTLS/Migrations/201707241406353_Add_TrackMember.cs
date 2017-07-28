namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_TrackMember : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrackMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MacAddress = c.String(),
                        VisitedDateTime = c.DateTime(nullable: false),
                        AreaName = c.String(),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TrackMembers");
        }
    }
}
