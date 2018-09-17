namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class organaizationAndDesignation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        OrganaizationId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organaizations", t => t.OrganaizationId, cascadeDelete: true)
                .Index(t => t.OrganaizationId);
            
            CreateTable(
                "dbo.Organaizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Designations", "OrganaizationId", "dbo.Organaizations");
            DropIndex("dbo.Designations", new[] { "OrganaizationId" });
            DropTable("dbo.Organaizations");
            DropTable("dbo.Designations");
        }
    }
}
