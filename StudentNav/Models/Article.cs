using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentNav.Models
{
    public class Article
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime DatePosted { get; set; }
        public MediaType MediaType { get; set; }
        public virtual ISet<ArticleMediaContent> MediaContents { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public ArticleType ArticleType { get; set; }
        public ApplicationUser Author { get; set; }

        public virtual ISet<Comment> Comments { get; set; }
        public virtual ISet<Likes> Likes { get; set; }
    }

    public enum ArticleType
    {
        General,DUT,WITS,UJ,UKZN,Guidance
    }
}