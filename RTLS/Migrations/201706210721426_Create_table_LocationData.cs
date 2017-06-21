namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_table_LocationData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mac = c.String(),
                        sequence = c.String(),
                        sn = c.String(),
                        bn = c.String(),
                        fn = c.String(),
                        x = c.Int(nullable: false),
                        y = c.Int(nullable: false),
                        z = c.Int(nullable: false),
                        last_seen_ts = c.Long(nullable: false),
                        action = c.String(),
                        fix_result = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LocationDatas");
        }
    }
}
