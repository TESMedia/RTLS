namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_MacAddress_tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MacAddresses", "Intstatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MacAddresses", "Intstatus");
        }
    }
}
