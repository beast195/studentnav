namespace StudentNav.Models
{
    public class Likes
    {
        public long Id { get; set; }
        public bool Like { get; set; }
        public ApplicationUser Liker { get; set; }
        public LikeType LikeType { get; set; }
        public BlogPost BlogPost { get; set; }
        public Article Article { get; set; }
    }

    public enum LikeType
    {
        Blog,Article
    }
}