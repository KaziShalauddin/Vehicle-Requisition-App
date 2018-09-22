namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequsitionTableRequsitionNumberFieldAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requsitions", "RequsitionNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requsitions", "RequsitionNumber");
        }
    }
}
