namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagerTableJourneyStartAndEndDateAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Managers", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Managers", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Managers", "EndDate");
            DropColumn("dbo.Managers", "StartDate");
        }
    }
}
