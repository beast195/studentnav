using Microsoft.AspNet.Identity;
using StudentNav.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace StudentNav.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public ActionResult Index()
        //{
        //    var articles = db.Articles.Select(s => new ArticleViewModel
        //    {
        //        Id = s.Id,
        //        DatePosted = s.DatePosted,
        //        Title = s.Title,
        //        Content = s.Content,
        //        MediaLinks = s.MediaContents.Select(c => c.MediaLink).ToList()
        //    }).ToList();
        //    foreach (var article in articles)
        //    {
        //        article.Description = GetDescription(article.Content);
        //    }
        //    return View(articles);
        //}



        public ViewResult Index(string search = null, int? page = null)
        {
            var currentpage = page ?? 0;

            if (!String.IsNullOrEmpty(search))
            {
                currentpage = 0;
            }
            var recordCount = db.Articles.Where(sc => sc.ArticleType == ArticleType.General).Count();
            if (recordCount < currentpage)
            {
                currentpage = 0;
            }
            List<ArticleViewModel> articles = null;
            if (!String.IsNullOrEmpty(search))
            {
                articles = db.Articles.Where(sc => sc.ArticleType == ArticleType.General && ( sc.Title.Contains(search) || sc.Content.Contains(search) || sc.Author.FirstName.Contains(search)))
                   .Include(c => c.MediaContents).Include(c => c.Author).Select(s => new ArticleViewModel
                   {
                       Id = s.Id,
                       Title = s.Title.Length > 35 ? s.Title.Substring(0, 35) + "..." : s.Title,
                       MediaLinks = s.MediaContents.Select(c => c.MediaLink).ToList(),
                       DatePosted = s.DatePosted,
                       Author = s.Author,
                       Content = s.Content
                   }).ToList();
            }
            else
            {
                articles = db.Articles.Where(c=>c.ArticleType == ArticleType.General)
                    .Include(c => c.MediaContents).Include(c => c.Author).Select(s => new ArticleViewModel
                    {
                        Id = s.Id,
                        Title = s.Title.Length > 35 ? s.Title.Substring(0, 35)+"..." : s.Title,
                        MediaLinks = s.MediaContents.Select(c => c.MediaLink).ToList(),
                        DatePosted = s.DatePosted,
                        Author = s.Author,
                        Content = s.Content
                    })     
                .OrderByDescending(o => o.DatePosted).Skip(currentpage).Take(6).ToList();
            }

            foreach (var article in articles)
            {
                article.Description = GetDescription(article.Content);
            }

            ViewBag.TotalNumberOfPageS = recordCount;
            ViewBag.NextPage = currentpage + 6;
            ViewBag.Search = search;
            return View(articles);
        }

        
        public string GetDescription(string cont)
        {
            var matches = Regex.Matches(cont, @"[\w\s]+");
            foreach (Match match in matches)
            {
                if (match.Value.Count() > 40)
                {
                    if (match.Value.Length > 39)
                    {
                        return match.Value.Substring(0, 40) + " ..."; ;
                    }
                    return match.Value + " ...";
                }
            }

            return "No Description";
        }

        [Authorize(Roles = "Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DropDownModal()
        {
            ViewBag.Message = "Your contact page.";

            return PartialView();
        }

        public ActionResult Article()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Article([Bind(Include = "MyFiles,Title,ArticleType,Content")] UploadArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var article = new Article();
                var user = db.Users.Where(c => c.Id == userId).FirstOrDefault();
                article.Author = user;
                article.Content = model.Content;
                article.DatePosted = DateTime.Now;
                var mediaContents = new List<ArticleMediaContent>();
                if (!model.MyFiles.Contains(null))
                {
                    foreach (HttpPostedFileBase upload in model.MyFiles)
                    {
                        var fileName = Path.GetFileName(upload.FileName);
                        var serverPath = Path.Combine(Server.MapPath("/Images/ArticlePics/" + fileName));
                        upload.SaveAs(serverPath);
                        var mediaContent = new ArticleMediaContent
                        {
                            ContentType = upload.ContentType,
                            MediaType = upload.ContentType.Contains("image") ? MediaType.Images : MediaType.Videos,
                            MediaLink = "/Images/ArticlePics/" + fileName
                        };
                        mediaContents.Add(mediaContent);
                    }

                    article.MediaContents = new HashSet<ArticleMediaContent>(mediaContents);
                }
                article.Title = model.Title;
                article.ArticleType = model.ArticleType;
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}