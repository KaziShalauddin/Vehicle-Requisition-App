namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vehicleModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.VehicleTypes", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VehicleTypes", "IsDeleted");
            DropColumn("dbo.Vehicles", "IsDeleted");
        }
    }
}
