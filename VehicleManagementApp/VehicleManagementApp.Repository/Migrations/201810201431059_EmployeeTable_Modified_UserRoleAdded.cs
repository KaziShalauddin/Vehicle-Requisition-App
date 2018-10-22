namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeTable_Modified_UserRoleAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "UserRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "UserRole");
        }
    }
}
