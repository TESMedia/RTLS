namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New_Changes_To_Rtls_Configuration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations");
            DropPrimaryKey("dbo.RtlsConfigurations");
            DropColumn("dbo.RtlsConfigurations", "Id");
            AddColumn("dbo.RtlsConfigurations", "RtlsConfigurationId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.RtlsConfigurations", "RtlsConfigurationId");
            AddForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations", "RtlsConfigurationId");
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.RtlsConfigurations", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations");
            DropPrimaryKey("dbo.RtlsConfigurations");
            DropColumn("dbo.RtlsConfigurations", "RtlsConfigurationId");
            AddPrimaryKey("dbo.RtlsConfigurations", "Id");
            AddForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations", "Id");
        }
    }
}
