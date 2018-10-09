namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleStatusTable_And_DriverStatusTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DriverStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Status = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.VehicleStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Status = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleStatus", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.DriverStatus", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.VehicleStatus", new[] { "VehicleId" });
            DropIndex("dbo.DriverStatus", new[] { "EmployeeId" });
            DropTable("dbo.VehicleStatus");
            DropTable("dbo.DriverStatus");
        }
    }
}
