namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlogPostImplementationsFixs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                        MediaType = c.Int(nullable: false),
                        Content = c.String(),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                        Content = c.String(),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Comments = c.String(),
                        BlogPost_Id = c.Long(),
                        Commentor_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPosts", t => t.BlogPost_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Commentor_Id)
                .Index(t => t.BlogPost_Id)
                .Index(t => t.Commentor_Id);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Like = c.Boolean(nullable: false),
                        BlogPost_Id = c.Long(),
                        Liker_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPosts", t => t.BlogPost_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Liker_Id)
                .Index(t => t.BlogPost_Id)
                .Index(t => t.Liker_Id);
            
            CreateTable(
                "dbo.MediaContents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MediaLink = c.String(),
                        ContentType = c.String(),
                        MediaType = c.Int(nullable: false),
                        BlogPost_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPosts", t => t.BlogPost_Id)
                .Index(t => t.BlogPost_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MediaContents", "BlogPost_Id", "dbo.BlogPosts");
            DropForeignKey("dbo.Likes", "Liker_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "BlogPost_Id", "dbo.BlogPosts");
            DropForeignKey("dbo.Comments", "Commentor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "BlogPost_Id", "dbo.BlogPosts");
            DropForeignKey("dbo.BlogPosts", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Articles", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.MediaContents", new[] { "BlogPost_Id" });
            DropIndex("dbo.Likes", new[] { "Liker_Id" });
            DropIndex("dbo.Likes", new[] { "BlogPost_Id" });
            DropIndex("dbo.Comments", new[] { "Commentor_Id" });
            DropIndex("dbo.Comments", new[] { "BlogPost_Id" });
            DropIndex("dbo.BlogPosts", new[] { "Author_Id" });
            DropIndex("dbo.Articles", new[] { "Author_Id" });
            DropTable("dbo.MediaContents");
            DropTable("dbo.Likes");
            DropTable("dbo.Comments");
            DropTable("dbo.BlogPosts");
            DropTable("dbo.Articles");
        }
    }
}
