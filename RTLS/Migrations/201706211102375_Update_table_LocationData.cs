namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_table_LocationData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LocationDatas", "AreaName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LocationDatas", "AreaName");
        }
    }
}
