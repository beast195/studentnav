using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentNav.Models
{
    public class FileUploadsViewModel
    {
        public List<string> ArticlePicFiles { get; set; }
        public List<string> AdPics { get; set; }
        public List<HttpPostedFileWrapper> uploads { get; set; }
        public string ArticleOrAds { get; set; }
        public int Adsthing { get; set; }

    }
}