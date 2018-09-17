namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequsitionManagerAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequsitionId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: false)
                .ForeignKey("dbo.Requsitions", t => t.RequsitionId, cascadeDelete: false)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: false)
                .Index(t => t.RequsitionId)
                .Index(t => t.VehicleId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Managers", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Managers", "RequsitionId", "dbo.Requsitions");
            DropForeignKey("dbo.Managers", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Managers", new[] { "EmployeeId" });
            DropIndex("dbo.Managers", new[] { "VehicleId" });
            DropIndex("dbo.Managers", new[] { "RequsitionId" });
            DropTable("dbo.Managers");
        }
    }
}
