namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Requsition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requsitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Form = c.String(),
                        To = c.String(),
                        JourneyStart = c.DateTime(nullable: false),
                        JouneyEnd = c.DateTime(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                        RequsitionStatusId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: false)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: false)
                .ForeignKey("dbo.RequsitionStatus", t => t.RequsitionStatusId, cascadeDelete: false)
                .Index(t => t.EmployeeId)
                .Index(t => t.CommentId)
                .Index(t => t.RequsitionStatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requsitions", "RequsitionStatusId", "dbo.RequsitionStatus");
            DropForeignKey("dbo.Requsitions", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Requsitions", "CommentId", "dbo.Comments");
            DropIndex("dbo.Requsitions", new[] { "RequsitionStatusId" });
            DropIndex("dbo.Requsitions", new[] { "CommentId" });
            DropIndex("dbo.Requsitions", new[] { "EmployeeId" });
            DropTable("dbo.Requsitions");
        }
    }
}
