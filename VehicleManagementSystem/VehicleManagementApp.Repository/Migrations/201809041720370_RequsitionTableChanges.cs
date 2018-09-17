namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequsitionTableChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requsitions", "RequsitionStatusId", "dbo.RequsitionStatus");
            DropIndex("dbo.Requsitions", new[] { "RequsitionStatusId" });
            DropColumn("dbo.Requsitions", "RequsitionStatusId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requsitions", "RequsitionStatusId", c => c.Int());
            CreateIndex("dbo.Requsitions", "RequsitionStatusId");
            AddForeignKey("dbo.Requsitions", "RequsitionStatusId", "dbo.RequsitionStatus", "Id");
        }
    }
}
