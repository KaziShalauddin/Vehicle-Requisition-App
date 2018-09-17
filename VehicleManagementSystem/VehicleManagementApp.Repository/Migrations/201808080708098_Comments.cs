namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comments = c.String(),
                        CommntId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.CommntId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.CommntId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Comments", "CommntId", "dbo.Comments");
            DropIndex("dbo.Comments", new[] { "EmployeeId" });
            DropIndex("dbo.Comments", new[] { "CommntId" });
            DropTable("dbo.Comments");
        }
    }
}
