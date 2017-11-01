namespace RTLS.Domins.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_SiteFloor_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteFloors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteFloorName = c.String(maxLength: 70, unicode: false, storeType: "nvarchar"),
                        FlooeImagePath = c.String(maxLength: 70, unicode: false, storeType: "nvarchar"),
                        XRange = c.Int(nullable: false),
                        YRange = c.Int(nullable: false),
                        ScaleFactor = c.Int(nullable: false),
                        DisplayConfiguration = c.String(maxLength: 50, unicode: false, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SiteFloors");
        }
    }
}
