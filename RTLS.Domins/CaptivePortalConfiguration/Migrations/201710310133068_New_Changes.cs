namespace RTLS.Domins.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_Changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RtlsConfigurations", "DisplayType", c => c.Int(nullable: false));
            AddColumn("dbo.RtlsConfigurations", "Port", c => c.Int(nullable: false));
            AddColumn("dbo.RtlsConfigurations", "PulisherSocketHostAddress", c => c.String(maxLength: 100, unicode: false, storeType: "nvarchar"));
            AddColumn("dbo.SiteFloors", "RtlsConfigureId", c => c.Int(nullable: false));
            AlterColumn("dbo.RtlsConfigurations", "SiteName", c => c.String(nullable: false, maxLength: 50, unicode: false, storeType: "nvarchar"));
            CreateIndex("dbo.SiteFloors", "RtlsConfigureId");
            AddForeignKey("dbo.SiteFloors", "RtlsConfigureId", "dbo.RtlsConfigurations", "RtlsConfigurationId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiteFloors", "RtlsConfigureId", "dbo.RtlsConfigurations");
            DropIndex("dbo.SiteFloors", new[] { "RtlsConfigureId" });
            AlterColumn("dbo.RtlsConfigurations", "SiteName", c => c.String(nullable: false, unicode: false));
            DropColumn("dbo.SiteFloors", "RtlsConfigureId");
            DropColumn("dbo.RtlsConfigurations", "PulisherSocketHostAddress");
            DropColumn("dbo.RtlsConfigurations", "Port");
            DropColumn("dbo.RtlsConfigurations", "DisplayType");
        }
    }
}
