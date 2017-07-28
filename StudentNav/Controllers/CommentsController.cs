using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StudentNav.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

//using System.Web.Http;

namespace StudentNav.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
      
        // GET: Comments
        public ActionResult Index()
        {
            return View(db.Comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(int blogId)
        {
            ViewBag.BlogId = blogId;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Comments,BlogPostId,ArticleId")] CommentViewModel commentModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var comment = new Comment();
                var commentor = db.Users.Where(c => c.Id == userId).First();
                if(commentModel.ArticleId == null)
                {
                    var blog = db.BlogPosts.Where(c => c.Id == commentModel.BlogPostId).First();
                    comment.BlogPost = blog;
                    comment.Commentor = commentor;
                    comment.Comments = commentModel.Comments;
                    comment.CommentType = CommentType.Blog;
                    comment.Date = DateTime.Now;
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return RedirectToAction($"Details/{commentModel.BlogPostId}", "BlogPosts");
                }
                else
                {
                    var article = db.Articles.Find(commentModel.ArticleId);
                    comment.Article = article;
                    comment.Commentor = commentor;
                    comment.Comments = commentModel.Comments;
                    comment.CommentType = CommentType.Article;
                    comment.Date = DateTime.Now;
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return RedirectToAction($"Details/{commentModel.ArticleId}", "Articles");
                }
               
                
            }

            return View(commentModel);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Comments")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        //[HttpGet]
        //public IHttpActionResult Hello()
        //{
        //    var name = "just";
        //    var n = name + "in";
        //   return  Ok();
        //}
        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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