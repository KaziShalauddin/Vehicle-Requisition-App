namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsTable_Modified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "SenderEmployeeId", c => c.Int());
            AddColumn("dbo.Comments", "ReceiverEmployeeId", c => c.Int());
            AddColumn("dbo.Comments", "IsReceiverSeen", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comments", "ReceiverSeenTime", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Comments", "SenderEmployeeId");
            CreateIndex("dbo.Comments", "ReceiverEmployeeId");
            AddForeignKey("dbo.Comments", "ReceiverEmployeeId", "dbo.Employees", "Id");
            AddForeignKey("dbo.Comments", "SenderEmployeeId", "dbo.Employees", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "SenderEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Comments", "ReceiverEmployeeId", "dbo.Employees");
            DropIndex("dbo.Comments", new[] { "ReceiverEmployeeId" });
            DropIndex("dbo.Comments", new[] { "SenderEmployeeId" });
            DropColumn("dbo.Comments", "ReceiverSeenTime");
            DropColumn("dbo.Comments", "IsReceiverSeen");
            DropColumn("dbo.Comments", "ReceiverEmployeeId");
            DropColumn("dbo.Comments", "SenderEmployeeId");
        }
    }
}
