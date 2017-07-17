namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_LocationData_tbl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LocationDatas", "last_seen_ts", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LocationDatas", "last_seen_ts", c => c.Long(nullable: false));
        }
    }
}
