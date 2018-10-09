namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleStatusTable_Modified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VehicleStatus", "ManagerId", c => c.Int());
            CreateIndex("dbo.VehicleStatus", "ManagerId");
            AddForeignKey("dbo.VehicleStatus", "ManagerId", "dbo.Managers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleStatus", "ManagerId", "dbo.Managers");
            DropIndex("dbo.VehicleStatus", new[] { "ManagerId" });
            DropColumn("dbo.VehicleStatus", "ManagerId");
        }
    }
}
