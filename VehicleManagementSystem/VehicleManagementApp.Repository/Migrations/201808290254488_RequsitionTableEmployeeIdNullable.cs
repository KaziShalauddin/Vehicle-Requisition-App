namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequsitionTableEmployeeIdNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Requsitions", new[] { "EmployeeId" });
            AlterColumn("dbo.Requsitions", "EmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Requsitions", "EmployeeId");
            AddForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Requsitions", new[] { "EmployeeId" });
            AlterColumn("dbo.Requsitions", "EmployeeId", c => c.Int());
            CreateIndex("dbo.Requsitions", "EmployeeId");
            AddForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees", "Id");
        }
    }
}
