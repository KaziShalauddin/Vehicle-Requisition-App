namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagerTableStatusFieldAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Managers", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Managers", "Status");
        }
    }
}
