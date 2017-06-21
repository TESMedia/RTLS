namespace RTLS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MacAddress_tbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MacAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mac = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MacAddresses");
        }
    }
}
