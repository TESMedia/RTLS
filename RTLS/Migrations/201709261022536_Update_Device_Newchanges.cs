namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Device_Newchanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations");
            DropIndex("dbo.Devices", new[] { "RtlsConfigureId" });
            AlterColumn("dbo.Devices", "RtlsConfigureId", c => c.Int());
            CreateIndex("dbo.Devices", "RtlsConfigureId");
            AddForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations");
            DropIndex("dbo.Devices", new[] { "RtlsConfigureId" });
            AlterColumn("dbo.Devices", "RtlsConfigureId", c => c.Int(nullable: false));
            CreateIndex("dbo.Devices", "RtlsConfigureId");
            AddForeignKey("dbo.Devices", "RtlsConfigureId", "dbo.RtlsConfigurations", "Id", cascadeDelete: true);
        }
    }
}
