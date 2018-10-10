namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DriverStatusTable_Modified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DriverStatus", "RequsitionId", c => c.Int());
            CreateIndex("dbo.DriverStatus", "RequsitionId");
            AddForeignKey("dbo.DriverStatus", "RequsitionId", "dbo.Requsitions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DriverStatus", "RequsitionId", "dbo.Requsitions");
            DropIndex("dbo.DriverStatus", new[] { "RequsitionId" });
            DropColumn("dbo.DriverStatus", "RequsitionId");
        }
    }
}
