namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Requisition_Modified_ForShowNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requsitions", "IsEmployeeSeen", c => c.Boolean(nullable: false));
            AddColumn("dbo.Requsitions", "IsAssigned", c => c.Boolean(nullable: false));
            AddColumn("dbo.Requsitions", "IsReAssigned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requsitions", "IsReAssigned");
            DropColumn("dbo.Requsitions", "IsAssigned");
            DropColumn("dbo.Requsitions", "IsEmployeeSeen");
        }
    }
}
