namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequsitionStatusAndRoleTableRemoveFromProject : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.RequsitionStatus");
            DropTable("dbo.Roles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequsitionStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
