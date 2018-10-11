namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleTable_NameField_Added_For_DetailedName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "Name");
        }
    }
}
