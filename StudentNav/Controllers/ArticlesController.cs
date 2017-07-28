using StudentNav.Models;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StudentNav.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Articles
        public ActionResult Index()
        {
            return View(db.Articles.ToList());
        }

        // GET: Articles/Details/5
        [AllowAnonymous]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            var articleViewModel = new ArticleViewModel
            {
                Id = article.Id,
                DatePosted = article.DatePosted,
                Title = article.Title,
                Content = article.Content,
                MediaLinks = article.MediaContents.Select(c => c.MediaLink).ToList(),                
                Likes = article.Likes.Where(l => l.Like).ToList()
            };
            articleViewModel.Comments = db.Comments.Where(l => l.Article.Id == article.Id).Include(k => k.Article).Include(k => k.Commentor).ToList();
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(articleViewModel);
        }

        [Authorize(Roles = "Admin,ContentProvider")]
        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,ContentProvider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,DatePosted,MediaType,Content")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: BlogPosts/Edit/5
        [Authorize]
        public ActionResult Edit(long? id, string fromdets = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var article = db.Articles.Find(id);
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(c => c.Id == userid).FirstOrDefault();
            var like = db.Likes.Where(c => c.BlogPost.Id == id && c.Liker.Id == userid).FirstOrDefault();
            if (like == null)
            {
                like = new Likes();
                like.Article = article;
                like.Like = true;
                like.LikeType = LikeType.Article;
                like.Liker = user;
                db.Likes.Add(like);
            }
            else
            {
                var howIfelt = like.Like;
                like.Like = !howIfelt;//not how I feel now :)
                db.Entry(like).State = EntityState.Modified;
            }

            db.SaveChanges();
            if (fromdets == null)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction($"Details/{id}", "Articles");
        }


        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,DatePosted,MediaType,Content")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Article article = db.Articles.Find(id);
            var mediaCon = db.ArticleMediaContents.Where(c => c.Article.Id == id).FirstOrDefault();
            if (mediaCon != null)
                db.ArticleMediaContents.Remove(mediaCon);

            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}