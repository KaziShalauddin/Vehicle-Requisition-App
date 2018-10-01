namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeTable_UserId_Modified : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "UserId", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "UserId", c => c.String());
        }
    }
}
