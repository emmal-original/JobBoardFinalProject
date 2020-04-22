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
    public class UserDetailsController : Controller
    {
        private FinalProjectEntities db = new FinalProjectEntities();

        // GET: UserDetails
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            var currentUser = from u in db.UserDetails
                              where u.UserId == userID
                              select u;
            return View(currentUser);
        }

        // GET: UserDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // GET: UserDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,ResumeFilename")] UserDetail userDetail, HttpPostedFileBase fupResume)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload CREATE
                string resumeFileName = "noPDF.pdf";

                //if user uploads resume, process it
                if (fupResume != null)
                {
                    resumeFileName = fupResume.FileName;

                    string ext = resumeFileName.Substring(resumeFileName.LastIndexOf("."));

                    if (ext.ToLower() == ".pdf")
                    {
                        //resumeFileName = Guid.NewGuid() + ext;
                        resumeFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fupResume.FileName;

                        fupResume.SaveAs(Server.MapPath("~/Content/Documents/EmployeeResumes/" + resumeFileName));

                        TempData["InvalidFileType"] = null;
                    }
                    else
                    {
                        TempData["InvalidFileType"] = "Only .pdf file types accepted. Please upload your resume in .pdf format to apply for open positions.";

                        resumeFileName = "noPDF.pdf";

                        return RedirectToAction("Index", "UserDetails");
                    }
                }

                userDetail.ResumeFilename = resumeFileName;

                #endregion

                db.UserDetails.Add(userDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDetail);
        }

        // GET: UserDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,ResumeFilename")] UserDetail userDetail, HttpPostedFileBase fupResume)
        {
            if (ModelState.IsValid)
            {
                #region FileUploadEDIT

                if (fupResume != null)
                {
                    string resumeFileName = fupResume.FileName;

                    string ext = resumeFileName.Substring(resumeFileName.LastIndexOf("."));

                    if (ext.ToLower() == ".pdf")
                    {
                        //resumeFileName = Guid.NewGuid() + ext;
                        resumeFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fupResume.FileName;

                        fupResume.SaveAs(Server.MapPath("~/Content/Documents/EmployeeResumes/" + resumeFileName));

                        TempData["InvalidFileType"] = null;

                        //Delete old image on file
                        if (userDetail.ResumeFilename != null && userDetail.ResumeFilename != "noPDF.pdf")
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/Documents/EmployeeResumes/" + userDetail.ResumeFilename));
                        }
                        userDetail.ResumeFilename = resumeFileName;
                    }
                    else
                    {
                        TempData["InvalidFileType"] = "Only .pdf file types accepted. Please upload your resume in .pdf format to apply for open positions.";

                        return RedirectToAction("Index", "UserDetails");
                    }
                }

                #endregion
                db.Entry(userDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userDetail);
        }

        // GET: UserDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserDetail userDetail = db.UserDetails.Find(id);
            db.UserDetails.Remove(userDetail);
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
