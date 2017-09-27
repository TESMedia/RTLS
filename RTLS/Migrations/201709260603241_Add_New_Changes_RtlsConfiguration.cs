namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_New_Changes_RtlsConfiguration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MacAddress = c.String(),
                        status = c.Int(nullable: false),
                        IsCreatedByAdmin = c.Boolean(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        IsDisplay = c.Boolean(nullable: false),
                        RtlsConfigureId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RtlsConfigurations", t => t.RtlsConfigureId, cascadeDelete: true)
                .Index(t => t.RtlsConfigureId);
            
            AddColumn("dbo.RtlsConfigurations", "SiteId", c => c.Int());
            DropColumn("dbo.RtlsConfigurations", "EngageBaseApi");
            DropTable("dbo.MacAddresses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MacAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mac = c.String(),
                        status = c.Int(nullable: false),
                        IsCreatedByAdmin = c.Boolean(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        IsDisplay = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RtlsConfigurations", "EngageBaseApi", c => c.String(maxLength: 500));
            DropForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations");
            DropIndex("dbo.Devices", new[] { "RtlsConfigureId" });
            DropColumn("dbo.RtlsConfigurations", "SiteId");
            DropTable("dbo.Devices");
        }
    }
}
