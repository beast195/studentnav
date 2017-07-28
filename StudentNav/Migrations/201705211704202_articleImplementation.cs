namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articleImplementation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleMediaContents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MediaLink = c.String(),
                        ContentType = c.String(),
                        MediaType = c.Int(nullable: false),
                        Article_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.Article_Id)
                .Index(t => t.Article_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleMediaContents", "Article_Id", "dbo.Articles");
            DropIndex("dbo.ArticleMediaContents", new[] { "Article_Id" });
            DropTable("dbo.ArticleMediaContents");
        }
    }
}
