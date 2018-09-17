namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequsitionTableAddedColumnStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requsitions", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requsitions", "Status");
        }
    }
}
