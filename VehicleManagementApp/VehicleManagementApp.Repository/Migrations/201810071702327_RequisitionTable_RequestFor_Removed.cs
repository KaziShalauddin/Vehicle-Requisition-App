namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionTable_RequestFor_Removed : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Requsitions", "RequestFor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requsitions", "RequestFor", c => c.Int());
        }
    }
}
