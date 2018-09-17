namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentId_Nullable_tblComment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Managers", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Comments", new[] { "CommntId" });
            DropIndex("dbo.Managers", new[] { "EmployeeId" });
            AlterColumn("dbo.Comments", "CommntId", c => c.Int());
            AlterColumn("dbo.Managers", "EmployeeId", c => c.Int());
            CreateIndex("dbo.Comments", "CommntId");
            CreateIndex("dbo.Managers", "EmployeeId");
            AddForeignKey("dbo.Managers", "EmployeeId", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Managers", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Managers", new[] { "EmployeeId" });
            DropIndex("dbo.Comments", new[] { "CommntId" });
            AlterColumn("dbo.Managers", "EmployeeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Comments", "CommntId", c => c.Int(nullable: false));
            CreateIndex("dbo.Managers", "EmployeeId");
            CreateIndex("dbo.Comments", "CommntId");
            AddForeignKey("dbo.Managers", "EmployeeId", "dbo.Employees", "Id", cascadeDelete: true);
        }
    }
}
