namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Requisition_Modified_IsHoldAdded_ForShowNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requsitions", "IsHold", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requsitions", "IsHold");
        }
    }
}
