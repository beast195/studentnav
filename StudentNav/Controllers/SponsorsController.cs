using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StudentNav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentNav.Controllers
{
    public class SponsorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Sponsors
        public ActionResult Index()
        {
            return View();
        }

        // GET: Sponsors/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Sponsors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sponsors/Create
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

        // GET: Sponsors/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Sponsors/Edit/5
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

        // GET: Sponsors/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Sponsors/Delete/5
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
        public ActionResult Bursary()
        {
            return View(db.SponsorLinks.Where(s=>s.LinkType == LinkType.Bursary).ToList());
        }

        public ActionResult EmploymentAgencies()
        {
            return View(db.SponsorLinks.Where(s => s.LinkType == LinkType.Agency).ToList());
        }

    }
}
