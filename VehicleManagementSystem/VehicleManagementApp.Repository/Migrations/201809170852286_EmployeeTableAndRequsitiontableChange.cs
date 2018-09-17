namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeTableAndRequsitiontableChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requsitions", "ToPlace", c => c.String());
            AddColumn("dbo.Employees", "EmployeeName", c => c.String(maxLength: 255));
            DropColumn("dbo.Requsitions", "To");
            DropColumn("dbo.Employees", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Name", c => c.String(maxLength: 255));
            AddColumn("dbo.Requsitions", "To", c => c.String());
            DropColumn("dbo.Employees", "EmployeeName");
            DropColumn("dbo.Requsitions", "ToPlace");
        }
    }
}
