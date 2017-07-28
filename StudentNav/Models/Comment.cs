using System;

namespace StudentNav.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public ApplicationUser Commentor { get; set; }
        public string Comments { get; set; }
        public BlogPost BlogPost { get; set; }

        public Article Article { get; set; }

        public CommentType CommentType { get; set; }
        public DateTime Date { get; set; }
    }

    public enum CommentType
    {
        Blog,Article
    }
}