namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThanaAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Thanas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DistrictId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: false)
                .Index(t => t.DistrictId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Thanas", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Thanas", new[] { "DistrictId" });
            DropTable("dbo.Thanas");
        }
    }
}
