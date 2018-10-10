namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleStatusTable_Modified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VehicleStatus", "RequsitionId", c => c.Int());
            CreateIndex("dbo.VehicleStatus", "RequsitionId");
            AddForeignKey("dbo.VehicleStatus", "RequsitionId", "dbo.Requsitions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleStatus", "RequsitionId", "dbo.Requsitions");
            DropIndex("dbo.VehicleStatus", new[] { "RequsitionId" });
            DropColumn("dbo.VehicleStatus", "RequsitionId");
        }
    }
}
