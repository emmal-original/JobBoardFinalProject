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
using PagedList;
using PagedList.Mvc;

namespace JobBoardFinalProject.UI.MVC.Controllers
{
    public class OpenPositionsController : Controller
    {
        private FinalProjectEntities db = new FinalProjectEntities();

        // GET: OpenPositions
        [Authorize]
        public ActionResult Index()
        {
            string currentUserID = User.Identity.GetUserId();
            var userAppliedFor = db.Applications.Where(x => x.UserId == currentUserID);
            var openPositions = db.OpenPositions.Include(o => o.Location).Include(o => o.Position).Include(o => o.Applications);

            List<int> positionsAppliedFor = new List<int>();

            foreach (var x in userAppliedFor)
            {
                foreach (var y in openPositions)
                {
                    if (x.OpenPositionId == y.OpenPositionId)
                    {
                        positionsAppliedFor.Add(y.OpenPositionId);
                    }
                }
            }

            ViewBag.PositionsAppliedFor = positionsAppliedFor;

            return View(openPositions.ToList());
        }

        // GET: OpenPositions
        [Authorize]
        public ViewResult IndexTile(string searchString, string currentFilter, int page=1)
        {
            string currentUserID = User.Identity.GetUserId();
            var userAppliedFor = db.Applications.Where(x => x.UserId == currentUserID);
            var openPositions = db.OpenPositions.Include(o => o.Location).Include(o => o.Position).Include(o => o.Applications);

            List<int> positionsAppliedFor = new List<int>();

            foreach (var x in userAppliedFor)
            {
                foreach (var y in openPositions)
                {
                    if (x.OpenPositionId == y.OpenPositionId)
                    {
                        positionsAppliedFor.Add(y.OpenPositionId);
                    }
                }
            }

            ViewBag.PositionsAppliedFor = positionsAppliedFor;

            int pageSize = 12;

            var openPositionsList = db.OpenPositions.ToList();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                openPositionsList = (from x in openPositionsList
                                     where x.Position.Title.ToLower().Contains(searchString.ToLower()) ||
                        x.Position.Department.DepartmentName.ToLower().Contains(searchString.ToLower()) ||
                        x.Location.City.ToLower().Contains(searchString.ToLower()) ||
                        x.Location.State.ToLower().Contains(searchString.ToLower())
                        select x).ToList();
            }

            return View(openPositionsList.ToPagedList(page, pageSize));
        }


        //[Authorize(Roles ="Admin, Manager")]
        //public ActionResult ManagerOpenPositions()
        //{
        //    if (User.IsInRole("Manager"))
        //    {
        //        string userID = User.Identity.GetUserId();
        //        var managerLocation = from op in db.OpenPositions
        //                              where op.Location.ManagerId == userID
        //                              select op;
        //        return View(managerLocation.ToList());
        //    }
        //    else
        //    {
        //        var openPositions = db.OpenPositions.Include(o => o.Location).Include(o => o.Position);
        //        return View(openPositions.ToList());
        //    }
        //}

        //POST: Apply
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Apply(int id)
        {
            Application app = new Application();

            string currentUserID = User.Identity.GetUserId();
            var user = (from ud in db.UserDetails
                       where ud.UserId == currentUserID
                       select ud).FirstOrDefault();
            string resume = user.ResumeFilename;

            app.UserId = currentUserID;
            app.OpenPositionId = id;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationStatusId = 1;
            app.ResumeFilename = resume;
            var userAppliedFor = db.Applications.Where(x => x.UserId == currentUserID && x.OpenPositionId == id).FirstOrDefault();

            if (resume == null)
            {
                TempData["ResumeErrorMessage"] = "Resume required to apply for open positions. Please upload your resume.";
                return RedirectToAction("Index", "UserDetails");
            }
            else
            {
                TempData["ResumeErrorMessage"] = null;

                if (userAppliedFor != null)
                {
                    TempData["DuplicateApplicationMessage"] = "You have already applied for this Open Position. Please review your applications.";

                    return RedirectToAction("Index", "Applications");
                }
                else
                {
                    TempData["DuplicateApplicationMessage"] = null;

                    if (ModelState.IsValid)
                    {
                        db.Applications.Add(app);
                        db.SaveChanges();

                        TempData["SuccessMessage"] = "Thank you for your interest in this position. Please check back for updates on the status of your application.";

                        return RedirectToAction("Index", "Applications");
                    }
                }
            }
            return View();
        }



        // GET: OpenPositions/Details/5
        [Authorize]
        public ActionResult Details(int? id, string previousView)
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

            string currentUserID = User.Identity.GetUserId();
            var userAppliedFor = db.Applications.Where(x => x.UserId == currentUserID);
            var openPositions = db.OpenPositions.Include(o => o.Location).Include(o => o.Position).Include(o => o.Applications);

            List<int> positionsAppliedFor = new List<int>();

            foreach (var x in userAppliedFor)
            {
                foreach (var y in openPositions)
                {
                    if (x.OpenPositionId == y.OpenPositionId)
                    {
                        positionsAppliedFor.Add(y.OpenPositionId);
                    }
                }
            }

            ViewBag.PositionsAppliedFor = positionsAppliedFor;


            ViewBag.BackToView = previousView;

            return View(openPosition);
        }

        // GET: OpenPositions/Details/5 FROM PAGED LIST
        //[Authorize]
        //public ActionResult DetailsFromIndexTile(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    OpenPosition openPosition = db.OpenPositions.Find(id);
        //    if (openPosition == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(openPositionsList.ToPagedList(page, pageSize));
        //}


        // GET: OpenPositions/Create
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationID", "BranchNumber");
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title");

            return View();
        }

        // POST: OpenPositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Create([Bind(Include = "OpenPositionId,LocationId,PositionId,PostingDate")] OpenPosition openPosition)
        {

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

        // GET: OpenPositions/Edit/5
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
            ViewBag.LocationId = new SelectList(db.Locations, "LocationID", "BranchNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // POST: OpenPositions/Edit/5
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

        // GET: OpenPositions/Delete/5
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

        // POST: OpenPositions/Delete/5
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
