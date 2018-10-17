namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentTableUserIdandUserNameAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "UserId", c => c.String());
            AddColumn("dbo.Comments", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "UserId");
            DropColumn("dbo.Comments", "UserName");
        }
    }
}
