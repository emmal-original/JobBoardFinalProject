using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoardFinalProject.DATA.EF;
using Microsoft.AspNet.Identity;

namespace JobBoardFinalProject.UI.MVC.Controllers
{
    public class ManageOpenPositionsController : Controller
    {
        private FinalProjectEntities db = new FinalProjectEntities();

        // GET: ManageOpenPositions
        [Authorize(Roles ="Admin, Manager")]
        public ActionResult Index()
        {
            if (User.IsInRole("Manager"))
            {
                string userID = User.Identity.GetUserId();
                var managerLocation = from op in db.OpenPositions
                                      where op.Location.ManagerId == userID
                                      select op;

                return View(managerLocation.ToList());
            }
            else
            {
                var openPositions = db.OpenPositions.Include(o => o.Location).Include(o => o.Position);
                return View(openPositions.ToList());
            }
        }

        // GET: ManageOpenPositions/Details/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenPosition openPosition = db.OpenPositions.Find(id);
            if (openPosition == null)
            {
                return HttpNotFound();
            }
            return View(openPosition);
        }

        // GET: ManageOpenPositions/Create
        [Authorize(Roles ="Manager")]
        public ActionResult Create()
        {
            string currentUser = User.Identity.GetUserId();
            ViewBag.LocationId = new SelectList(db.Locations.Where(x=> x.ManagerId == currentUser), "LocationID", "BranchNumber");
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title");

            //DateTime postingDate = DateTime.Today;
            //openPosition.PostingDate = postingDate;

            return View();
        }

        // POST: ManageOpenPositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Create([Bind(Include = "OpenPositionId,LocationId,PositionId,PostingDate")] OpenPosition openPosition)
        {
            DateTime postingDate = new DateTime();
            postingDate = DateTime.Today;
            openPosition.PostingDate = postingDate;

            if (ModelState.IsValid)
            {
                db.OpenPositions.Add(openPosition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationID", "BranchNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // GET: ManageOpenPositions/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenPosition openPosition = db.OpenPositions.Find(id);
            if (openPosition == null)
            {
                return HttpNotFound();
            }

            string currentUser = User.Identity.GetUserId();
            ViewBag.LocationId = new SelectList(db.Locations.Where(x=>x.ManagerId == currentUser), "LocationID", "BranchNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // POST: ManageOpenPositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit([Bind(Include = "OpenPositionId,LocationId,PositionId,PostingDate")] OpenPosition openPosition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(openPosition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationID", "BranchNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // GET: ManageOpenPositions/Delete/5
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenPosition openPosition = db.OpenPositions.Find(id);
            if (openPosition == null)
            {
                return HttpNotFound();
            }
            return View(openPosition);
        }

        // POST: ManageOpenPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            OpenPosition openPosition = db.OpenPositions.Find(id);
            db.OpenPositions.Remove(openPosition);
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
