using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ARMS.Data;
using ARMS.Data.Helpers;
using ARMS.Data.Models;
using ARMS.Helpers;
using ARMS.ViewModel;

namespace ARMS.Controllers
{
    public class CoursesController : Controller
    {
        private ArmsContext db = new ArmsContext();

        // GET: Courses
        public ActionResult Index(int userId)
        {
            List<Course> courses = null;

            if(CurrentWebContext.CurrentUser.Type == "student")
                courses = CourseHelper.GetCoursesForParticipant(userId);

            if (CurrentWebContext.CurrentUser.Type == "teacher")
                courses = CourseHelper.GetCoursesForSupervisor(userId);

            return View(courses.ToList());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int courseId = (int)id;

            Course course = CourseHelper.GetById(courseId);
            if (course == null)
            {
                return HttpNotFound();
            }

            var vm = new DetailedCourseVM(course)
            {
                Supervisors = SupervisorHelper.GetSupervisorsForCourse(courseId),
                Lectures = SupervisorHelper.GetLecturesForCourse(courseId),
                Participants = SupervisorHelper.GetParticipantsForCourse(courseId)
            };

            return View(vm);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.CreatorID = new SelectList(db.Users, "UserID", "LastName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,CourseName,CourseDescription,CreatorID")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();

                var model = new Supervisor(CurrentWebContext.CurrentUser.UserID, course.CourseID);
                model.Insert();

                return RedirectToAction("Index", new { userId = CurrentWebContext.CurrentUser.UserID });
            }

            ViewBag.CreatorID = new SelectList(db.Users, "UserID", "LastName", course.CreatorID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int courseId = (int)id;

            Course course = CourseHelper.GetById(courseId);
            if (course == null)
            {
                return HttpNotFound();
            }

            // check if the user trying to access the course is a supervisor
            var supervisors = SupervisorHelper.GetSupervisorsForCourse(courseId);

            // if he isn't, then return him => denied
            if(!supervisors.Any(x => x.UserID == CurrentWebContext.CurrentUser.UserID))
                {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var vm = new DetailedCourseVM(course)
            {
                Supervisors = supervisors,
                Lectures = SupervisorHelper.GetLecturesForCourse(courseId),
                Participants = SupervisorHelper.GetParticipantsForCourse(courseId)
            };

            return View(vm);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,CourseName,CourseDescription,CreatorID, Supervisors, Participants, Lectures")] DetailedCourseVM model)
        {
            if (ModelState.IsValid)
            {
                var course = new Course() { CourseID = model.CourseID, CourseName = model.CourseName, CourseDescription = model.CourseDescription, CreatorID = model.CreatorID, Creator = model.Creator};
                course.Update();
            }

            return RedirectToAction("Index", new { userId = CurrentWebContext.CurrentUser.UserID });
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index", new { userId = CurrentWebContext.CurrentUser.UserID });
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
