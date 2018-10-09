namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriverStatusTable_Modified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DriverStatus", "ManagerId", c => c.Int());
            CreateIndex("dbo.DriverStatus", "ManagerId");
            AddForeignKey("dbo.DriverStatus", "ManagerId", "dbo.Managers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DriverStatus", "ManagerId", "dbo.Managers");
            DropIndex("dbo.DriverStatus", new[] { "ManagerId" });
            DropColumn("dbo.DriverStatus", "ManagerId");
        }
    }
}
