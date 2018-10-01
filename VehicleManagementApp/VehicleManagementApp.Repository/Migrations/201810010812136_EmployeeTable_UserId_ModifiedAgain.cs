namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeTable_UserId_ModifiedAgain : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "UserId", c => c.Guid());
        }
    }
}
