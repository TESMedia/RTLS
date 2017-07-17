namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes_MacAddress_Tbl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MacAddresses", "IsCreatedByAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LocationDatas", "last_seen_ts", c => c.String());
        }
    }
}
