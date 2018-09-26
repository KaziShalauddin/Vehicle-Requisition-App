namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DesignationTable_Modified : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Designations", "OrganaizationId", "dbo.Organaizations");
            DropIndex("dbo.Designations", new[] { "OrganaizationId" });
            AddColumn("dbo.Designations", "DepartmentId", c => c.Int());
            CreateIndex("dbo.Designations", "DepartmentId");
            AddForeignKey("dbo.Designations", "DepartmentId", "dbo.Departments", "Id");
            DropColumn("dbo.Designations", "OrganaizationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Designations", "OrganaizationId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Designations", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Designations", new[] { "DepartmentId" });
            DropColumn("dbo.Designations", "DepartmentId");
            CreateIndex("dbo.Designations", "OrganaizationId");
            AddForeignKey("dbo.Designations", "OrganaizationId", "dbo.Organaizations", "Id", cascadeDelete: true);
        }
    }
}
