namespace RTLS.Domins.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_changes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Thread = c.String(unicode: false),
                        Level = c.String(unicode: false),
                        Logger = c.String(unicode: false),
                        Message = c.String(unicode: false),
                        Exception = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrackMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MacAddress = c.String(unicode: false),
                        VisitedDateTime = c.DateTime(nullable: false, precision: 0),
                        PostDateTime = c.DateTime(nullable: false, precision: 0),
                        RecieveDateTime = c.DateTime(nullable: false, precision: 0),
                        AreaName = c.String(unicode: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MacAddress = c.String(unicode: false),
                        status = c.Int(nullable: false),
                        IsCreatedByAdmin = c.Boolean(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false, precision: 0),
                        IsDisplay = c.Boolean(nullable: false),
                        RtlsConfigureId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RtlsConfigurations", t => t.RtlsConfigureId)
                .Index(t => t.RtlsConfigureId);
            
            CreateTable(
                "dbo.RtlsConfigurations",
                c => new
                    {
                        RtlsConfigurationId = c.Int(nullable: false, identity: true),
                        EngageSiteName = c.String(nullable: false, maxLength: 50, unicode: false, storeType: "nvarchar"),
                        EngageBuildingName = c.String(nullable: false, maxLength: 50, unicode: false, storeType: "nvarchar"),
                        EngageBaseAddressUri = c.String(nullable: false, maxLength: 200, unicode: false, storeType: "nvarchar"),
                        SiteId = c.Int(nullable: false),
                        SiteName = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.RtlsConfigurationId);
            
            CreateTable(
                "dbo.LocationDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mac = c.String(unicode: false),
                        sequence = c.String(unicode: false),
                        sn = c.String(unicode: false),
                        bn = c.String(unicode: false),
                        fn = c.String(unicode: false),
                        x = c.Int(nullable: false),
                        y = c.Int(nullable: false),
                        z = c.Int(nullable: false),
                        last_seen_ts = c.String(unicode: false),
                        action = c.String(unicode: false),
                        fix_result = c.String(unicode: false),
                        AreaName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations");
            DropIndex("dbo.Devices", new[] { "RtlsConfigureId" });
            DropTable("dbo.LocationDatas");
            DropTable("dbo.RtlsConfigurations");
            DropTable("dbo.Devices");
            DropTable("dbo.TrackMembers");
            DropTable("dbo.AppLogs");
        }
    }
}
