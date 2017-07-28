namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articleTypeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "ArticleType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "ArticleType");
        }
    }
}
