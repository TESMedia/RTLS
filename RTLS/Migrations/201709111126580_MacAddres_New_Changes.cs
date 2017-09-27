namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MacAddres_New_Changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MacAddresses", "status", c => c.Int(nullable: false));
            DropColumn("dbo.MacAddresses", "Intstatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MacAddresses", "Intstatus", c => c.Int(nullable: false));
            DropColumn("dbo.MacAddresses", "status");
        }
    }
}
