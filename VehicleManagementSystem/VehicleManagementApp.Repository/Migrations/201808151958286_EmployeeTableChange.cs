namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeTableChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Address1", c => c.String(maxLength: 255));
            AddColumn("dbo.Employees", "Address2", c => c.String());
            AddColumn("dbo.Employees", "DivisionId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "DistrictId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "ThanaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "DivisionId");
            CreateIndex("dbo.Employees", "DistrictId");
            CreateIndex("dbo.Employees", "ThanaId");
            AddForeignKey("dbo.Employees", "DistrictId", "dbo.Districts", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Employees", "DivisionId", "dbo.Divisions", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Employees", "ThanaId", "dbo.Thanas", "Id", cascadeDelete: false);
            DropColumn("dbo.Employees", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Address", c => c.String(maxLength: 255));
            DropForeignKey("dbo.Employees", "ThanaId", "dbo.Thanas");
            DropForeignKey("dbo.Employees", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Employees", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Employees", new[] { "ThanaId" });
            DropIndex("dbo.Employees", new[] { "DistrictId" });
            DropIndex("dbo.Employees", new[] { "DivisionId" });
            DropColumn("dbo.Employees", "ThanaId");
            DropColumn("dbo.Employees", "DistrictId");
            DropColumn("dbo.Employees", "DivisionId");
            DropColumn("dbo.Employees", "Address2");
            DropColumn("dbo.Employees", "Address1");
        }
    }
}
