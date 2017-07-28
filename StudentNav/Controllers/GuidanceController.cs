using StudentNav.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace StudentNav.Controllers
{
    public class GuidanceController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Guidance
        public ViewResult Index(string search = null, int? page = null)
        {
            var currentpage = page ?? 0;

            if (!String.IsNullOrEmpty(search))
            {
                currentpage = 0;
            }
            var recordCount = db.Articles.Where(c=>c.ArticleType == ArticleType.Guidance).Count();
            if (recordCount < currentpage)
            {
                currentpage = 0;
            }
            List<ArticleViewModel> articles = null;
            if (!String.IsNullOrEmpty(search))
            {
                articles = db.Articles.Where(sc => sc.ArticleType == ArticleType.Guidance && (sc.Title.Contains(search) || sc.Content.Contains(search) || sc.Author.FirstName.Contains(search)))
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
                articles = db.Articles.Where(c => c.ArticleType == ArticleType.Guidance)
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
                        return match.Value.Substring(0, 40) + " ..."; ;
                    }
                    return match.Value + " ..."; 
                }
            }

            return "No Description";
        }


        // GET: Guidance/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Guidance/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Guidance/Create
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

        // GET: Guidance/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Guidance/Edit/5
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

        // GET: Guidance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Guidance/Delete/5
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

        // GET: Guidance/Delete/5
        public ActionResult Career()
        {
            return View();
        }

        // GET: Guidance/Delete/5
        public ActionResult StudentTips()
        {
            return View();
        }
    }
}