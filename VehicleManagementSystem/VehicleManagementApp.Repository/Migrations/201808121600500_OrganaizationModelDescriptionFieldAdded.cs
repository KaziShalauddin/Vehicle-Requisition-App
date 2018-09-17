namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganaizationModelDescriptionFieldAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organaizations", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organaizations", "Description");
        }
    }
}
