namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changevihicletable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "VModel", c => c.String());
            AddColumn("dbo.Vehicles", "VRegistrationNo", c => c.String());
            AddColumn("dbo.Vehicles", "VChesisNo", c => c.String());
            AddColumn("dbo.Vehicles", "VCapacity", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "VCapacity");
            DropColumn("dbo.Vehicles", "VChesisNo");
            DropColumn("dbo.Vehicles", "VRegistrationNo");
            DropColumn("dbo.Vehicles", "VModel");
        }
    }
}
