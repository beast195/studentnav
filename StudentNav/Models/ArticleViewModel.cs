using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentNav.Models
{
    public class ArticleViewModel
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
        public ApplicationUser Author { get; set; }
    }

    public class UploadArticleViewModel
    {
        public string Title { get; set; }
        public ArticleType ArticleType { get; set; }
        public List<HttpPostedFileWrapper> MyFiles { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}