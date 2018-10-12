namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentTableTimeFieldAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "CommentTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "CommentTime");
        }
    }
}
