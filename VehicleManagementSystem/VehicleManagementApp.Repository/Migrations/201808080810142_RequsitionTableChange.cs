namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequsitionTableChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requsitions", "CommentId", "dbo.Comments");
            DropForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Requsitions", "RequsitionStatusId", "dbo.RequsitionStatus");
            DropIndex("dbo.Requsitions", new[] { "EmployeeId" });
            DropIndex("dbo.Requsitions", new[] { "CommentId" });
            DropIndex("dbo.Requsitions", new[] { "RequsitionStatusId" });
            AddColumn("dbo.Requsitions", "Description", c => c.String());
            AlterColumn("dbo.Requsitions", "EmployeeId", c => c.Int());
            AlterColumn("dbo.Requsitions", "CommentId", c => c.Int());
            AlterColumn("dbo.Requsitions", "RequsitionStatusId", c => c.Int());
            CreateIndex("dbo.Requsitions", "EmployeeId");
            CreateIndex("dbo.Requsitions", "CommentId");
            CreateIndex("dbo.Requsitions", "RequsitionStatusId");
            AddForeignKey("dbo.Requsitions", "CommentId", "dbo.Comments", "Id");
            AddForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees", "Id");
            AddForeignKey("dbo.Requsitions", "RequsitionStatusId", "dbo.RequsitionStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requsitions", "RequsitionStatusId", "dbo.RequsitionStatus");
            DropForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Requsitions", "CommentId", "dbo.Comments");
            DropIndex("dbo.Requsitions", new[] { "RequsitionStatusId" });
            DropIndex("dbo.Requsitions", new[] { "CommentId" });
            DropIndex("dbo.Requsitions", new[] { "EmployeeId" });
            AlterColumn("dbo.Requsitions", "RequsitionStatusId", c => c.Int(nullable: false));
            AlterColumn("dbo.Requsitions", "CommentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Requsitions", "EmployeeId", c => c.Int(nullable: false));
            DropColumn("dbo.Requsitions", "Description");
            CreateIndex("dbo.Requsitions", "RequsitionStatusId");
            CreateIndex("dbo.Requsitions", "CommentId");
            CreateIndex("dbo.Requsitions", "EmployeeId");
            AddForeignKey("dbo.Requsitions", "RequsitionStatusId", "dbo.RequsitionStatus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Requsitions", "CommentId", "dbo.Comments", "Id", cascadeDelete: true);
        }
    }
}
