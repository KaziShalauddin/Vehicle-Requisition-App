namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentTableUserIdRemove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Comments", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "UserId", c => c.String());
        }
    }
}
