using Microsoft.AspNet.Identity;
using StudentNav.Models;
using StudentNav.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentNav.Controllers
{
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ImageService imgService = new ImageService();

        public ViewResult Index(string search = null, int? page = null)
        {
            var currentpage = page ?? 0;

            if (!String.IsNullOrEmpty(search))
            {
                currentpage = 0;
            }
            var recordCount = db.BlogPosts.Count();
            if (recordCount < currentpage)
            {
                currentpage = 0;
            }
            List<BlogPostViewModel> blogs = null;
            if (!String.IsNullOrEmpty(search))
            {
                blogs = db.BlogPosts.Where(sc => sc.Title.Contains(search) || sc.Content.Contains(search) || sc.Author.FirstName.Contains(search))
                   .Include(c => c.MediaContents).Include(c => c.Author).Include(c => c.Likes).Include(c => c.Comments).Select(s => new BlogPostViewModel
                   {
                       Id = s.Id,
                       Title = s.Title.Length > 35 ? s.Title.Substring(0, 35) + "..." : s.Title,
                       MediaLinks = s.MediaContents.Select(c => c.MediaLink).ToList(),
                       DatePosted = s.DatePosted,
                       Author = s.Author.FirstName,
                       Content = s.Content,
                       Likes = s.Likes.Where(l => l.Like).ToList(),
                       Comments = s.Comments.ToList()
                   }).ToList();
            }
            else
            {
                blogs = db.BlogPosts.Include(c => c.MediaContents).Include(c => c.Author).Include(c => c.Likes).Include(c => c.Comments).Select(s => new BlogPostViewModel
                {
                    Id = s.Id,
                    Title = s.Title.Length > 35 ? s.Title.Substring(0, 35) + "..." : s.Title,
                    MediaLinks = s.MediaContents.Select(c => c.MediaLink).ToList(),
                    DatePosted = s.DatePosted,
                    Author = s.Author.FirstName,
                    Content = s.Content,
                    Likes = s.Likes.Where(l => l.Like).ToList(),
                    Comments = s.Comments.ToList()
                }).OrderByDescending(o => o.DatePosted).Skip(currentpage).Take(6).ToList();
            }

            foreach (var blog in blogs)
            {
                blog.Description = GetDescription(blog.Content);
            }

            ViewBag.TotalNumberOfPageS = recordCount;
            ViewBag.NextPage = currentpage + 6;
            ViewBag.Search = search;
            return View(blogs);
        }

        public string GetDescription(string cont)
        {
            
                if (cont.Count() > 40)
                {
                    if (cont.Length > 100)
                    {
                        return cont.Substring(0, 101) + " ..."; 
                    }
                    return cont + " ..."; 
                }
            

            return "No Description";
        }

        // GET: BlogPosts
        //public ActionResult Index()
        //{
        //    var blogs = db.BlogPosts.Include(c => c.MediaContents).Include(c => c.Author).Include(c => c.Likes).Include(c => c.Comments).Select(s => new BlogPostViewModel
        //    {
        //        Id = s.Id,
        //        Title = s.Title,
        //        MediaLinks = s.MediaContents.Select(c => c.MediaLink).ToList(),
        //        DatePosted = s.DatePosted,
        //        Author = s.Author.UserName,
        //        Content = s.Content,
        //        Likes = s.Likes.Where(l => l.Like).ToList(),
        //        Comments = s.Comments.ToList()
        //    }).ToList();

        //    return View(blogs);
        //}

        // GET: BlogPosts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var blog = db.BlogPosts.Where(b => b.Id == id).Include(c => c.MediaContents).Include(c => c.Author).Include(c => c.Likes).Select(s => new BlogPostViewModel
            {
                Id = s.Id,
                Title = s.Title,
                MediaLinks = s.MediaContents.Select(c => c.MediaLink).ToList(),
                DatePosted = s.DatePosted,
                Author = s.Author.FirstName,
                Content = s.Content,
                Likes = s.Likes.Where(l => l.Like).ToList()
            }).First();
            blog.Comments = db.Comments.Where(l => l.BlogPost.Id == blog.Id).Include(k => k.BlogPost).Include(k => k.Commentor).ToList();
            //BlogPost blogPost = db.BlogPosts.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: BlogPosts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "Title,MediaFiles,Content")] UploadBlogPostViewModel uploadBlogPost)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var user = db.Users.Where(c => c.Id == userid).FirstOrDefault();
                var blogPost = new BlogPost();
                blogPost.Author = user;
                blogPost.DatePosted = DateTime.Now;
                blogPost.Content = uploadBlogPost.Content;

                var mediaContents = new List<MediaContent>();
                foreach (HttpPostedFileBase upload in uploadBlogPost.MediaFiles)
                {
                    var imageLink = await imgService.CreatePhoto(upload.InputStream);
                    var mediaContent = new MediaContent
                    {
                        ContentType = upload.ContentType,
                        MediaType = MediaType.Images ,//upload.ContentType.Contains("image") ? MediaType.Images : MediaType.Videos,
                        MediaLink = imageLink
                    };
                    mediaContents.Add(mediaContent);
                }

                blogPost.MediaContents = new HashSet<MediaContent>(mediaContents);

                blogPost.Title = uploadBlogPost.Title;
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(uploadBlogPost);
        }

        // GET: BlogPosts/Edit/5
        [Authorize]
        public ActionResult Edit(long? id, string fromdets = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var blogPost = db.BlogPosts.Find(id);
            var userid = User.Identity.GetUserId();
            var user = db.Users.Where(c => c.Id == userid).FirstOrDefault();
            var like = db.Likes.Where(c => c.BlogPost.Id == id && c.Liker.Id == userid).FirstOrDefault();
            if (like == null)
            {
                like = new Likes();
                like.BlogPost = blogPost;
                like.Like = true;
                like.LikeType = LikeType.Blog;
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
            return RedirectToAction($"Details/{id}","BlogPosts");
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Title,DatePosted,MediaFiles,Content")] UploadBlogPostViewModel blogPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var blogPost = db.BlogPosts.Where(c => c.Id == id).Include(s => s.Likes).Include(s => s.Comments).Include(s => s.MediaContents).FirstOrDefault();
            //var mediaCon = db.MediaContents.Where(c => c.BlogPost.Id == id).FirstOrDefault();
            //db.MediaContents.Remove(mediaCon);
            db.BlogPosts.Remove(blogPost);
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