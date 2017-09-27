namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Remove_Area_tbl : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocationDatas", "AreaId", "dbo.Areas");
            DropIndex("dbo.LocationDatas", new[] { "AreaId" });
            DropColumn("dbo.LocationDatas", "AreaId");
            DropTable("dbo.Areas");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        AreaId = c.Int(nullable: false, identity: true),
                        AreaName = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AreaId);
            
            AddColumn("dbo.LocationDatas", "AreaId", c => c.Int());
            CreateIndex("dbo.LocationDatas", "AreaId");
            AddForeignKey("dbo.LocationDatas", "AreaId", "dbo.Areas", "AreaId");
        }
    }
}
