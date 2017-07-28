namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetDefaultRoles : DbMigration
    {
        public override void Up()
        {
            Sql("insert into AspNetRoles(id,name)  values (1,'Admin') ,(2,'CProvider') , (3,'User')");
        }
        
        public override void Down()
        {
        }
    }
}
