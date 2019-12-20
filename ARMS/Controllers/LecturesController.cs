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
    [Authorize]
    [RoutePrefix("Lectures")]
    public class LecturesController : Controller
    {
        // GET: Lectures/courseId
        public ActionResult Index(int courseId)
        {
            var lectures = LectureHelper.GetLecturesForCourse(courseId);

            ViewBag.CourseID = courseId;

            return View(lectures);
        }

        // GET: Lectures/Details/courseId&lectureId
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

        // GET: Lectures/Create/courseId
        public ActionResult Create(int courseId)
        {
            ViewBag.CourseID = courseId;
            return View();
        }

        // POST: Lectures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LectureID,From,To,CheckInEnabled,CourseID")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                if (lecture.From >= lecture.To)
                {
                    ViewBag.DateError = "Lecture from cannot be equal or after To";
                    return View(lecture);
                }

                lecture.Insert();
                return RedirectToAction("Index", new { courseId = lecture.CourseID });
            }
            return View(lecture);
        }

        // GET: Lectures/Edit/courseId&lectureId
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

        // POST: Lectures/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LectureID,From,To,CheckInEnabled,CourseID")] Lecture lecture)
        {
            if (ModelState.IsValid)
            {
                if(lecture.From >= lecture.To)
                {
                    ViewBag.DateError = "Lecture from cannot be equal or after To";
                    return View(lecture);
                }

                lecture.Insert();
                return RedirectToAction("Index", lecture.CourseID);
            }

            ViewBag.Attendees = AttendeeHelper.GetAttendeesForLecture(lecture.LectureID);

            return View(lecture);
        }

        // GET: Lectures/Delete/courseId&lectureId
        [Route("Lectures/Delete")]
        public ActionResult Delete(int courseId, int? lectureId)
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

            var success = lecture.Delete();
            ViewBag.Success = success;
            ViewBag.CourseID = courseId;

            return View();
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
