namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_tbl_MacAddress : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MacAddresses", "CompnayId", "dbo.Companies");
            DropIndex("dbo.MacAddresses", new[] { "CompnayId" });
            AddColumn("dbo.MacAddresses", "SiteId", c => c.Int());
            CreateIndex("dbo.MacAddresses", "SiteId");
            AddForeignKey("dbo.MacAddresses", "SiteId", "dbo.Sites", "SiteId");
            DropColumn("dbo.MacAddresses", "CompnayId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MacAddresses", "CompnayId", c => c.Int());
            DropForeignKey("dbo.MacAddresses", "SiteId", "dbo.Sites");
            DropIndex("dbo.MacAddresses", new[] { "SiteId" });
            DropColumn("dbo.MacAddresses", "SiteId");
            CreateIndex("dbo.MacAddresses", "CompnayId");
            AddForeignKey("dbo.MacAddresses", "CompnayId", "dbo.Companies", "CompnayId");
        }
    }
}
