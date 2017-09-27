namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_New_Changes_RtlsConfiguration_withSiteName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RtlsConfigurations", "SiteName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RtlsConfigurations", "SiteName");
        }
    }
}
