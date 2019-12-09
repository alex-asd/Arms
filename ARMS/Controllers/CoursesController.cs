﻿using System;
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
        public ActionResult Details(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int courseID = (int)courseId;

            Course course = CourseHelper.GetById(courseID);
            if (course == null)
            {
                return HttpNotFound();
            }

            var vm = new DetailedCourseVM(course)
            {
                Supervisors = SupervisorHelper.GetSupervisorsForCourse(courseID),
                Lectures = LectureHelper.GetLecturesForCourse(courseID),
                Participants = UserHelper.GetParticipantsForCourse(courseID)
            };

            ViewBag.CountOfPendingStudents = ParticipantHelper.GetCountOfPendingParticipants(courseID);

            return View(vm);
        }

        // GET: Courses/Create
        public ActionResult Create(int userId)
        {
            ViewBag.UserID = userId;
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
                course.Insert();

                var model = new Supervisor(CurrentWebContext.CurrentUser.UserID, course.CourseID);
                model.Insert();

                return RedirectToAction("Index", new { userId = CurrentWebContext.CurrentUser.UserID });
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int courseID = (int)courseId;

            Course course = CourseHelper.GetById(courseID);
            if (course == null)
            {
                return HttpNotFound();
            }

            // check if the user trying to access the course is a supervisor
            var supervisors = SupervisorHelper.GetSupervisorsForCourse(courseID);

            // if he isn't, then return him => denied
            if(!supervisors.Any(x => x.UserID == CurrentWebContext.CurrentUser.UserID))
                {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var vm = new DetailedCourseVM(course)
            {
                Supervisors = supervisors,
                Lectures = LectureHelper.GetLecturesForCourse(courseID),
                Participants = UserHelper.GetParticipantsForCourse(courseID)
            };

            ViewBag.CountOfPendingStudents = ParticipantHelper.GetCountOfPendingParticipants(courseID);

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
        public ActionResult Delete(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = CourseHelper.GetById((int)courseId);
            if (course == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index", new { userId = CurrentWebContext.CurrentUser.UserID });
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
