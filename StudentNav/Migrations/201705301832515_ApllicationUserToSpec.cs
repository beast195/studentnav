namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApllicationUserToSpec : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Race", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "InstitutionType", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "HighSchool", c => c.String());
            AddColumn("dbo.AspNetUsers", "FieldOfStudy", c => c.String());
            AddColumn("dbo.AspNetUsers", "Province", c => c.String());
            AddColumn("dbo.AspNetUsers", "LevelOfStudy", c => c.String());
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "LevelOfStudy");
            DropColumn("dbo.AspNetUsers", "Province");
            DropColumn("dbo.AspNetUsers", "FieldOfStudy");
            DropColumn("dbo.AspNetUsers", "HighSchool");
            DropColumn("dbo.AspNetUsers", "InstitutionType");
            DropColumn("dbo.AspNetUsers", "Race");
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "Surname");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
