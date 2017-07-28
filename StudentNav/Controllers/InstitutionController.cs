using StudentNav.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace StudentNav.Controllers
{
    public class InstitutionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Institution
        public ActionResult Index()
        {
            return View();
        }

        // GET: Institution/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Institution/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Institution/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Institution/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Institution/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Institution/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Institution/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DUT(string search = null, int? page = null)
        {
            var currentpage = page ?? 0;

            if (!String.IsNullOrEmpty(search))
            {
                currentpage = 0;
            }
            var recordCount = db.Articles.Where(c => c.ArticleType == ArticleType.DUT).Count();
            if (recordCount < currentpage)
            {
                currentpage = 0;
            }
            List<ArticleViewModel> articles = null;
            if (!String.IsNullOrEmpty(search))
            {
                articles = db.Articles.Where(sc => sc.ArticleType == ArticleType.DUT && (sc.Title.Contains(search) || sc.Content.Contains(search) || sc.Author.FirstName.Contains(search)))
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
                articles = db.Articles.Where(c => c.ArticleType == ArticleType.DUT)
                    .Include(c => c.MediaContents).Include(c => c.Author).Select(s => new ArticleViewModel
                    {
                        Id = s.Id,
                        Title = s.Title.Length > 35 ? s.Title.Substring(0, 35) + "..." : s.Title,
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

        public ActionResult UJ(string search = null, int? page = null)
        {
            var currentpage = page ?? 0;

            if (!String.IsNullOrEmpty(search))
            {
                currentpage = 0;
            }
            var recordCount = db.Articles.Where(c => c.ArticleType == ArticleType.UJ).Count();
            if (recordCount < currentpage)
            {
                currentpage = 0;
            }
            List<ArticleViewModel> articles = null;
            if (!String.IsNullOrEmpty(search))
            {
                articles = db.Articles.Where(sc => sc.ArticleType == ArticleType.UJ && (sc.Title.Contains(search) || sc.Content.Contains(search) || sc.Author.FirstName.Contains(search)))
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
                articles = db.Articles.Where(c => c.ArticleType == ArticleType.UJ)
                    .Include(c => c.MediaContents).Include(c => c.Author).Select(s => new ArticleViewModel
                    {
                        Id = s.Id,
                        Title = s.Title.Length > 35 ? s.Title.Substring(0, 35) + "..." : s.Title,
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

        public ActionResult UKZN(string search = null, int? page = null)
        {
            var currentpage = page ?? 0;

            if (!String.IsNullOrEmpty(search))
            {
                currentpage = 0;
            }
            var recordCount = db.Articles.Where(c => c.ArticleType == ArticleType.UKZN).Count();
            if (recordCount < currentpage)
            {
                currentpage = 0;
            }
            List<ArticleViewModel> articles = null;
            if (!String.IsNullOrEmpty(search))
            {
                articles = db.Articles.Where(sc => sc.ArticleType == ArticleType.UKZN && (sc.Title.Contains(search) || sc.Content.Contains(search) || sc.Author.FirstName.Contains(search)))
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
                articles = db.Articles.Where(c => c.ArticleType == ArticleType.UKZN)
                    .Include(c => c.MediaContents).Include(c => c.Author).Select(s => new ArticleViewModel
                    {
                        Id = s.Id,
                        Title = s.Title.Length > 35 ? s.Title.Substring(0, 35) + "..." : s.Title,
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

        public ViewResult WITS(string search = null, int? page = null)
        {
            var currentpage = page ?? 0;

            if (!String.IsNullOrEmpty(search))
            {
                currentpage = 0;
            }
            var recordCount = db.Articles.Where(c=>c.ArticleType == ArticleType.WITS).Count();
            if (recordCount < currentpage)
            {
                currentpage = 0;
            }
            List<ArticleViewModel> articles = null;
            if (!String.IsNullOrEmpty(search))
            {
                articles = db.Articles.Where(sc => sc.ArticleType == ArticleType.WITS && (sc.Title.Contains(search) || sc.Content.Contains(search) || sc.Author.FirstName.Contains(search)))
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
                articles = db.Articles.Where(c => c.ArticleType == ArticleType.WITS)
                    .Include(c => c.MediaContents).Include(c => c.Author).Select(s => new ArticleViewModel
                    {
                        Id = s.Id,
                        Title = s.Title.Length > 35 ? s.Title.Substring(0, 35) + "..." : s.Title,
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
                        return match.Value.Substring(0, 40) + " ...";
                    }
                    return match.Value + " ...";
                }
            }

            return "No Description";
        }
    }
}