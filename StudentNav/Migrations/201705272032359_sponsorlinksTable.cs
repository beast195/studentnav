namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sponsorlinksTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SponsorLinks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LinkImage = c.String(),
                        LinkName = c.String(),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SponsorLinks");
        }
    }
}
