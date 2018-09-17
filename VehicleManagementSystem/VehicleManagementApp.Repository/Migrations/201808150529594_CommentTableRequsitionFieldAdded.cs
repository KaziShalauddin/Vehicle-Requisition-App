namespace VehicleManagementApp.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentTableRequsitionFieldAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requsitions", "CommentId", "dbo.Comments");
            DropIndex("dbo.Requsitions", new[] { "CommentId" });
            AddColumn("dbo.Comments", "RequsitionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "RequsitionId");
            AddForeignKey("dbo.Comments", "RequsitionId", "dbo.Requsitions", "Id", cascadeDelete: false);
            DropColumn("dbo.Requsitions", "CommentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requsitions", "CommentId", c => c.Int());
            DropForeignKey("dbo.Comments", "RequsitionId", "dbo.Requsitions");
            DropIndex("dbo.Comments", new[] { "RequsitionId" });
            DropColumn("dbo.Comments", "RequsitionId");
            CreateIndex("dbo.Requsitions", "CommentId");
            AddForeignKey("dbo.Requsitions", "CommentId", "dbo.Comments", "Id");
        }
    }
}
