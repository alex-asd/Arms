using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ARMS.Data;
using ARMS.Data.Models;
using ARMS.Data.Helpers;

namespace ARMS.Controllers
{
    public class LecturesController : Controller
    {
        // GET: Lectures
        public ActionResult Index(int courseId)
        {
            var lectures = LectureHelper.GetLecturesForCourse(courseId);

            ViewBag.CourseID = courseId;

            return View(lectures);
        }

        // GET: Lectures/Details/5
        public ActionResult Details(int courseId, int? lectureId)
        {
            if (lectureId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // cast the nullable obj to int
            int id = (int)lectureId;

            Lecture lecture = LectureHelper.GetById(id);

            if (lecture == null)
            {
                return HttpNotFound();
            }

            ViewBag.Attendees = AttendeeHelper.GetAttendeesForLecture(id);

            return View(lecture);
        }

        // GET: Lectures/Create
        public ActionResult Create(int courseId)
        {
            ViewBag.CourseID = courseId;
            return View();
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LectureID,From,To,CheckInEnabled,CourseID")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                lecture.Insert();
                return RedirectToAction("Index", new { courseId = lecture.CourseID });
            }

            return View(lecture);
        }

        // GET: Lectures/Edit/5
        public ActionResult Edit(int courseId, int? lectureId)
        {
            if (lectureId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Lecture lecture = LectureHelper.GetById((int)lectureId);

            if (lecture == null)
            {
                return HttpNotFound();
            }

            ViewBag.Attendees = AttendeeHelper.GetAttendeesForLecture((int)lectureId);

            return View(lecture);
        }

        // POST: Lectures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LectureID,From,To,CheckInEnabled,CourseID")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                lecture.Insert();
                return RedirectToAction("Index", lecture.CourseID);
            }

            ViewBag.Attendees = AttendeeHelper.GetAttendeesForLecture(lecture.LectureID);

            return View(lecture);
        }

        // GET: Lectures/Delete/5
        public ActionResult Delete(int? lectureId)
        {
            if (lectureId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Lecture lecture = LectureHelper.GetById((int)lectureId);

            if (lecture == null)
            {
                return HttpNotFound();
            }

            lecture.Delete();

            return RedirectToAction("Index", lecture.CourseID);
        }

        protected override void Dispose(bool disposing)
        {
            ArmsContext db = new ArmsContext();

            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
