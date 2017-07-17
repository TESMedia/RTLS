namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_table_cmp_Site : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Companies", "SiteId", "dbo.Sites");
            DropIndex("dbo.Companies", new[] { "SiteId" });
            AddColumn("dbo.Sites", "CompanyId", c => c.Int());
            CreateIndex("dbo.Sites", "CompanyId");
            AddForeignKey("dbo.Sites", "CompanyId", "dbo.Companies", "CompnayId");
            DropColumn("dbo.Companies", "SiteId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Companies", "SiteId", c => c.Int());
            DropForeignKey("dbo.Sites", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Sites", new[] { "CompanyId" });
            DropColumn("dbo.Sites", "CompanyId");
            CreateIndex("dbo.Companies", "SiteId");
            AddForeignKey("dbo.Companies", "SiteId", "dbo.Sites", "SiteId");
        }
    }
}
