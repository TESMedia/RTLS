namespace RTLS.Domins.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_Changes_RtlsConfiguration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RtlsConfigurations", "DisplayConfiguration", c => c.String(maxLength: 50, unicode: false, storeType: "nvarchar"));
            DropColumn("dbo.SiteFloors", "DisplayConfiguration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SiteFloors", "DisplayConfiguration", c => c.String(maxLength: 50, unicode: false, storeType: "nvarchar"));
            DropColumn("dbo.RtlsConfigurations", "DisplayConfiguration");
        }
    }
}
