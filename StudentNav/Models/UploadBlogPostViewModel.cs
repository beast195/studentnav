using System;
using System.Collections.Generic;
using System.Web;

namespace StudentNav.Models
{
    public class UploadBlogPostViewModel
    {
        public string Title { get; set; }
        public List<HttpPostedFileWrapper> MediaFiles { get; set; }
        public string Content { get; set; }
    }
}