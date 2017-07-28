using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentNav.Models
{
    public class UpdateArticleViewModel
    {

        public string Title { get; set; }
        [Display(Name = "Images")]
        public List<HttpPostedFileWrapper> MediaFiles { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}