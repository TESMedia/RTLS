namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Company_Site_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompnayId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        SiteId = c.Int(),
                    })
                .PrimaryKey(t => t.CompnayId)
                .ForeignKey("dbo.Sites", t => t.SiteId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteName = c.String(),
                    })
                .PrimaryKey(t => t.SiteId);
            
            AddColumn("dbo.MacAddresses", "CompnayId", c => c.Int());
            CreateIndex("dbo.MacAddresses", "CompnayId");
            AddForeignKey("dbo.MacAddresses", "CompnayId", "dbo.Companies", "CompnayId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MacAddresses", "CompnayId", "dbo.Companies");
            DropForeignKey("dbo.Companies", "SiteId", "dbo.Sites");
            DropIndex("dbo.Companies", new[] { "SiteId" });
            DropIndex("dbo.MacAddresses", new[] { "CompnayId" });
            DropColumn("dbo.MacAddresses", "CompnayId");
            DropTable("dbo.Sites");
            DropTable("dbo.Companies");
        }
    }
}
