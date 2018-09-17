namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentModelChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Comments", new[] { "CommntId" });
            DropIndex("dbo.Comments", new[] { "EmployeeId" });
            AlterColumn("dbo.Comments", "CommntId", c => c.Int());
            CreateIndex("dbo.Comments", "CommntId");
            DropColumn("dbo.Comments", "EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "EmployeeId", c => c.Int(nullable: false));
            DropIndex("dbo.Comments", new[] { "CommntId" });
            AlterColumn("dbo.Comments", "CommntId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "EmployeeId");
            CreateIndex("dbo.Comments", "CommntId");
            AddForeignKey("dbo.Comments", "EmployeeId", "dbo.Employees", "Id", cascadeDelete: true);
        }
    }
}
