namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Some_Changes_To_Rtls_Configuration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RtlsConfigurations", "EngageSiteName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.RtlsConfigurations", "EngageBuildingName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.RtlsConfigurations", "EngageBaseAddressUri", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.RtlsConfigurations", "SiteId", c => c.Int(nullable: false));
            AlterColumn("dbo.RtlsConfigurations", "SiteName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RtlsConfigurations", "SiteName", c => c.String());
            AlterColumn("dbo.RtlsConfigurations", "SiteId", c => c.Int());
            AlterColumn("dbo.RtlsConfigurations", "EngageBaseAddressUri", c => c.String(maxLength: 500));
            AlterColumn("dbo.RtlsConfigurations", "EngageBuildingName", c => c.String(maxLength: 100));
            AlterColumn("dbo.RtlsConfigurations", "EngageSiteName", c => c.String(maxLength: 100));
        }
    }
}
