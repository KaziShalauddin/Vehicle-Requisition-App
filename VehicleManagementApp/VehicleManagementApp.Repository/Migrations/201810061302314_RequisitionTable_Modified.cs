namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionTable_Modified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requsitions", "RequestType", c => c.String());
            AddColumn("dbo.Requsitions", "RequestFor", c => c.Int());
            AddColumn("dbo.Requsitions", "RequestedBy", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requsitions", "RequestedBy");
            DropColumn("dbo.Requsitions", "RequestFor");
            DropColumn("dbo.Requsitions", "RequestType");
        }
    }
}
