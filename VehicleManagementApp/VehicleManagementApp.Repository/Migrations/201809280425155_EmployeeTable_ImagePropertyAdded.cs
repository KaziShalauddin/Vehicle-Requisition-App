namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeTable_ImagePropertyAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Image", c => c.Binary());
            AddColumn("dbo.Employees", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "ImagePath");
            DropColumn("dbo.Employees", "Image");
        }
    }
}
