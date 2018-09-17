namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeTableIsDriverAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "IsDriver", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "IsDriver");
        }
    }
}
