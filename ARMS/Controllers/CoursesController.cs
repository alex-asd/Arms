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
    [Authorize]
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Index(int userId)
        {
            List<Course> courses = null;

            if (CurrentWebContext.CurrentUser.Type == "student")
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

            var viewModel = new DetailedCourseVM();

            if(CurrentWebContext.CurrentUser.Type == "student")
            {
                ViewBag.IsParticipant = false;
                viewModel = new DetailedCourseVM(course);
                // if the user is not a teacher, is he a participant of the course
                if (CourseHelper.IsStudentPartOfCourse(CurrentWebContext.CurrentUser.UserID, viewModel.CourseID))
                    ViewBag.IsParticipant = true;
            }

            // check if the current user is a teacher and setup accordingly the view model
            if (CurrentWebContext.CurrentUser.Type == "teacher")
            {
                viewModel = new DetailedCourseVM(course)
                {
                    Supervisors = SupervisorHelper.GetSupervisorsForCourse(courseID),
                    Lectures = LectureHelper.GetLecturesForCourse(courseID),
                    Participants = UserHelper.GetParticipantsForCourse(courseID)
                };

                ViewBag.CountOfPendingStudents = ParticipantHelper.GetCountOfPendingParticipants(courseID);
            }
            return View(viewModel);
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
                course.CreatorID = CurrentWebContext.CurrentUser.UserID;
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

            var viewModel = DetailedCourseVM.CreateDetailedCourseVMW(course, supervisors);

            ViewBag.CountOfPendingStudents = ParticipantHelper.GetCountOfPendingParticipants(courseID);

            return View(viewModel);
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

            course.Delete();

            return View();
        }

        // GET: Courses/Enroll/userId?courseId
        public ActionResult Enroll(int userId, int? courseId)
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
            
            var participant = new Participant(userId, courseID, Participant.STATUS_PENDING);
            var success = participant.Insert();

            if (success)
                ViewBag.Message = "Successfully applied for the course, a course supervisor must approve you!";
            else
                ViewBag.Message = "Something went wrong! Please try again.";

            return View();
        }

        public ActionResult SeeDetailedOverview(int courseId, int studentId)
        {
            Course course = CourseHelper.GetById(courseId);
            User student = UserHelper.GetById(studentId);

            if (course == null || student == null)
            {
                return HttpNotFound();
            }

            ViewBag.AttendancePerformance = ParticipantHelper.GetParticipantAttendance(student.UserID, course.CourseID);

            var vm = DetailedOverviewForStudentVM.CreateDetailedOverviewForStudentVM(student, course);
            return View(vm);
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
