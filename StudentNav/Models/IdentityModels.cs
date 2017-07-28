using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentNav.Models
{
    public enum Gender
    {
        Male,Female,other
    }
    public enum InstitutionType
    {
        HighSchool,Tertiary
    }
    public enum Race
    {
        African,White,Coloured,Indian,other
    }
    
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImagePath { get; set; }
        public string Institution { get; set; }
        public string FirstName{ get; set; }
        public string Surname { get; set; }
        public Gender Gender { get; set; }
        public Race Race { get; set; }

        public InstitutionType InstitutionType { get; set; }

        public string HighSchool { get; set; }

        public string FieldOfStudy { get; set; }

        public string Province { get; set; }

        public int Grade { get; set; }

        public string LevelOfStudy { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleMediaContent> ArticleMediaContents { get; set; }
        public DbSet<MediaContent> MediaContents { get; set; }
        public DbSet<SponsorLinks> SponsorLinks { get; set; }
        

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
    }
}