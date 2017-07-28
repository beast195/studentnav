using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentNav.Models
{
    public class BlogPostViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime DatePosted { get; set; }
        public MediaType MediaType { get; set; }
        public List<string> MediaLinks { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Likes> Likes { get; set; }
        public string Author { get; set; }
    }
}