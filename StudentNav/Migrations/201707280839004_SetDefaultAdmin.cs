namespace StudentNav.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetDefaultAdmin : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [ProfileImagePath], [Institution], [FirstName], [Surname], [Gender], [Race], [InstitutionType], [HighSchool], [FieldOfStudy], [Province], [LevelOfStudy], [Grade]) VALUES ('1af60fc8-8f7a-4bc3-bd26-99e5bd1cd2a6', 'admin@studentnav.com', 0, 'ACD4VEkw2sB1T7c4zhCsRvf4ajbBhMy+jeh9hU6PCrZcu9my0qbv8gbmoY22NmCz3A==', '2b2ff331-500b-43e4-9239-d7743bf8d1d0', NULL, 0, 0, NULL, 1, 0, 'admin@studentnav.com', null, 'DUT', N'Sabelo', 'Ngcongo', 0, 0, 1, NULL, 'Information Technology', NULL, 'BTech', 0)");
            Sql("insert into AspNetUserRoles(userid,roleid)  values ('1af60fc8-8f7a-4bc3-bd26-99e5bd1cd2a6',1)");
        }
        
        public override void Down()
        {
        }
    }
}
