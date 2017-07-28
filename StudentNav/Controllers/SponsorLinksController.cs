using StudentNav.Models;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StudentNav.Controllers
{
    public class SponsorLinksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SponsorLinks
        public ActionResult Index()
        {
            return View(db.SponsorLinks.ToList());
        }

        // GET: SponsorLinks/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SponsorLinks sponsorLinks = db.SponsorLinks.Find(id);
            if (sponsorLinks == null)
            {
                return HttpNotFound();
            }
            return View(sponsorLinks);
        }

        // GET: SponsorLinks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SponsorLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LinkImage,LinkType,LinkName,Link")] SponsorLinkViewModel sponsorLinks)
        {
            if (ModelState.IsValid)
            {
                var sponsor = new SponsorLinks();

                var fileName = Path.GetFileName(sponsorLinks.LinkImage.FileName);
                var serverPath = Path.Combine(Server.MapPath("/Images/SponsorPics/" + fileName));
                sponsorLinks.LinkImage.SaveAs(serverPath);

                sponsor.Link = sponsorLinks.Link;
                sponsor.LinkImage = "/Images/SponsorPics/" + fileName;
                sponsor.LinkName = sponsorLinks.LinkName;
                sponsor.LinkType = sponsorLinks.LinkType;

                db.SponsorLinks.Add(sponsor);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(sponsorLinks);
        }

        // GET: SponsorLinks/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SponsorLinks sponsorLinks = db.SponsorLinks.Find(id);
            if (sponsorLinks == null)
            {
                return HttpNotFound();
            }
            return View(sponsorLinks);
        }

        // POST: SponsorLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LinkImage,LinkName,Link")] SponsorLinks sponsorLinks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sponsorLinks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sponsorLinks);
        }

        // GET: SponsorLinks/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SponsorLinks sponsorLinks = db.SponsorLinks.Find(id);
            if (sponsorLinks == null)
            {
                return HttpNotFound();
            }
            return View(sponsorLinks);
        }

        // POST: SponsorLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            SponsorLinks sponsorLinks = db.SponsorLinks.Find(id);
            db.SponsorLinks.Remove(sponsorLinks);
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