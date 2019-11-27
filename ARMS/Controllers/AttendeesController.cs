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

namespace ARMS.Controllers
{
    public class AttendeesController : Controller
    {
        private ArmsContext db = new ArmsContext();

        // GET: Attendees
        public ActionResult Index()
        {
            var attendees = db.Attendees.Include(a => a.Lecture).Include(a => a.User);
            return View(attendees.ToList());
        }

        // GET: Attendees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // GET: Attendees/Create
        public ActionResult Create()
        {
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "LectureID");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username");
            return View();
        }

        // POST: Attendees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AttendeeID,BluetoothAddress,UserID,LectureID")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                db.Attendees.Add(attendee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "LectureID", attendee.LectureID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", attendee.UserID);
            return View(attendee);
        }

        // GET: Attendees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "LectureID", attendee.LectureID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", attendee.UserID);
            return View(attendee);
        }

        // POST: Attendees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttendeeID,BluetoothAddress,UserID,LectureID")] Attendee attendee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LectureID = new SelectList(db.Lectures, "LectureID", "LectureID", attendee.LectureID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", attendee.UserID);
            return View(attendee);
        }

        // GET: Attendees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendee attendee = db.Attendees.Find(id);
            if (attendee == null)
            {
                return HttpNotFound();
            }
            return View(attendee);
        }

        // POST: Attendees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendee attendee = db.Attendees.Find(id);
            db.Attendees.Remove(attendee);
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
