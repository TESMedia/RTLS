namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Changes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Thread = c.String(),
                        Level = c.String(),
                        Logger = c.String(),
                        Message = c.String(),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrackMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MacAddress = c.String(),
                        VisitedDateTime = c.DateTime(nullable: false),
                        PostDateTime = c.DateTime(nullable: false),
                        RecieveDateTime = c.DateTime(nullable: false),
                        AreaName = c.String(),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfigurationParameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LocationDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mac = c.String(),
                        sequence = c.String(),
                        sn = c.String(),
                        bn = c.String(),
                        fn = c.String(),
                        x = c.Int(nullable: false),
                        y = c.Int(nullable: false),
                        z = c.Int(nullable: false),
                        last_seen_ts = c.String(),
                        action = c.String(),
                        fix_result = c.String(),
                        AreaName = c.String(),
                        AreaId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.AreaId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        AreaId = c.Int(nullable: false, identity: true),
                        AreaName = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AreaId);
            
            CreateTable(
                "dbo.MacAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mac = c.String(),
                        Intstatus = c.Int(nullable: false),
                        IsCreatedByAdmin = c.Boolean(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        IsDisplay = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocationDatas", "AreaId", "dbo.Areas");
            DropIndex("dbo.LocationDatas", new[] { "AreaId" });
            DropTable("dbo.MacAddresses");
            DropTable("dbo.Areas");
            DropTable("dbo.LocationDatas");
            DropTable("dbo.ConfigurationParameters");
            DropTable("dbo.TrackMembers");
            DropTable("dbo.AppLogs");
        }
    }
}
