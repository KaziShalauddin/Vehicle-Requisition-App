namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleTableAddFieldStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "Status");
        }
    }
}
