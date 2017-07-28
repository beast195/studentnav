using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentNav.Models
{
    public class ArticleMediaContent
    {
        public long Id { get; set; }
        public string MediaLink { get; set; }
        public string ContentType { get; set; }
        public MediaType MediaType { get; set; }
        public Article Article { get; set; }
    }
}