namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Department : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OrganaizationId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organaizations", t => t.OrganaizationId, cascadeDelete: false)
                .Index(t => t.OrganaizationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "OrganaizationId", "dbo.Organaizations");
            DropIndex("dbo.Departments", new[] { "OrganaizationId" });
            DropTable("dbo.Departments");
        }
    }
}
