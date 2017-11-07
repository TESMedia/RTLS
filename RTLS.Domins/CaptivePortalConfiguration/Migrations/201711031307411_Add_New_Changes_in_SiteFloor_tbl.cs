namespace RTLS.Domins.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_New_Changes_in_SiteFloor_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SiteFloors", "XRangeFeedData", c => c.Int(nullable: false));
            AddColumn("dbo.SiteFloors", "YRangeFeedData", c => c.Int(nullable: false));
            AddColumn("dbo.SiteFloors", "CanvasXLength", c => c.Int(nullable: false));
            AddColumn("dbo.SiteFloors", "CanvasYLength", c => c.Int(nullable: false));
            AddColumn("dbo.SiteFloors", "TopStyle", c => c.Int(nullable: false));
            AddColumn("dbo.SiteFloors", "LeftStyle", c => c.Int(nullable: false));
            AddColumn("dbo.SiteFloors", "ImageXLength", c => c.Int(nullable: false));
            AddColumn("dbo.SiteFloors", "ImageYLength", c => c.Int(nullable: false));
            DropColumn("dbo.SiteFloors", "XRange");
            DropColumn("dbo.SiteFloors", "YRange");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SiteFloors", "YRange", c => c.Int(nullable: false));
            AddColumn("dbo.SiteFloors", "XRange", c => c.Int(nullable: false));
            DropColumn("dbo.SiteFloors", "ImageYLength");
            DropColumn("dbo.SiteFloors", "ImageXLength");
            DropColumn("dbo.SiteFloors", "LeftStyle");
            DropColumn("dbo.SiteFloors", "TopStyle");
            DropColumn("dbo.SiteFloors", "CanvasYLength");
            DropColumn("dbo.SiteFloors", "CanvasXLength");
            DropColumn("dbo.SiteFloors", "YRangeFeedData");
            DropColumn("dbo.SiteFloors", "XRangeFeedData");
        }
    }
}
