namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentAndLikesToArticles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "MediaType", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "CommentType", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "Article_Id", c => c.Long());
            AddColumn("dbo.Likes", "LikeType", c => c.Int(nullable: false));
            AddColumn("dbo.Likes", "Article_Id", c => c.Long());
            CreateIndex("dbo.Comments", "Article_Id");
            CreateIndex("dbo.Likes", "Article_Id");
            AddForeignKey("dbo.Comments", "Article_Id", "dbo.Articles", "Id");
            AddForeignKey("dbo.Likes", "Article_Id", "dbo.Articles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.Comments", "Article_Id", "dbo.Articles");
            DropIndex("dbo.Likes", new[] { "Article_Id" });
            DropIndex("dbo.Comments", new[] { "Article_Id" });
            DropColumn("dbo.Likes", "Article_Id");
            DropColumn("dbo.Likes", "LikeType");
            DropColumn("dbo.Comments", "Article_Id");
            DropColumn("dbo.Comments", "CommentType");
            DropColumn("dbo.BlogPosts", "MediaType");
        }
    }
}
