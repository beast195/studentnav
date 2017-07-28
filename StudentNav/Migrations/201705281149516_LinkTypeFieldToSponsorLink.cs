namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LinkTypeFieldToSponsorLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SponsorLinks", "LinkType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SponsorLinks", "LinkType");
        }
    }
}
