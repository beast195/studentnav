using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentNav.Models
{
    public enum MediaType
    {
        Images,Videos
    }
    public class BlogPost
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime DatePosted { get; set; }
        public virtual ISet<MediaContent> MediaContents { get; set; }
        public string Content { get; set; }
        public virtual ISet<Comment> Comments { get; set; }
        public virtual ISet<Likes> Likes { get; set; }
        public MediaType MediaType { get; set; }
        public ApplicationUser Author { get; set; }

    }
}