namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDateOfBirth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
        }
    }
}
