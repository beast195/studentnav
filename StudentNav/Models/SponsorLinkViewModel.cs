using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentNav.Models
{
    public class SponsorLinkViewModel
    {
        public long Id { get; set; }
        public HttpPostedFileWrapper LinkImage { get; set; }
        public string LinkName { get; set; }
        public string Link { get; set; }
        public LinkType LinkType { get; set; }
    }
}