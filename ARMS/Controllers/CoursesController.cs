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
        // GET: Courses/userId
        // returns view containing all courses related to the logged in user
        public ActionResult Index(int userId)
        {
            List<Course> courses = null;

            if (CurrentWebContext.CurrentUser.Type == "student")
                courses = CourseHelper.GetCoursesForParticipant(userId);

            if (CurrentWebContext.CurrentUser.Type == "teacher")
                courses = CourseHelper.GetCoursesForSupervisor(userId);

            return View(courses.ToList());
        }

        // GET: Courses/Details/courseId
        // returns details view with all related data depending on the logged in user
        public ActionResult Details(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // cast nullable int
            int id = (int)courseId;
            Course course = CourseHelper.GetById(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            DetailedCourseVM viewModel = null;

            // check if the current user is a student and setup accordingly the view model
            if (CurrentWebContext.CurrentUser.Type == "student")
            {
                ViewBag.IsParticipant = false;
                viewModel = new DetailedCourseVM(course);
                viewModel.Lectures = LectureHelper.GetLecturesForCourse(course.CourseID);
                // get the type of student for this course
                ViewBag.StatusOfStudent = CourseHelper.GetStudentStatusForCourse(CurrentWebContext.CurrentUser.UserID, course.CourseID);
            }

            // check if the current user is a teacher and create the view model for him
            if (CurrentWebContext.CurrentUser.Type == "teacher")
            {
                viewModel = DetailedCourseVM.CreateDetailedCourseVMW(course);
                ViewBag.CountOfPendingStudents = ParticipantHelper.GetCountOfPendingParticipants(id);
            }
            return View(viewModel);
        }

        // GET: Courses/Create/userId
        // returns create view, where a teacher can  create a course
        public ActionResult Create(int userId)
        {
            // if a student tries to access the url
            if(CurrentWebContext.CurrentUser.Type == "student")
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            ViewBag.UserID = userId;
            return View();
        }

        // POST: Courses/Create/course
        // retrievs the new course and creates it if the state is valid
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,CourseName,CourseDescription,CreatorID")] Course course)
        {
            if (ModelState.IsValid)
            {
                if(CourseHelper.Exists(course.CourseName))
                {
                    ViewBag.Error = "Such a course name exists, please try with another!";
                    return View();
                }

                course.CreatorID = CurrentWebContext.CurrentUser.UserID;
                course.Insert();

                var model = new Supervisor(CurrentWebContext.CurrentUser.UserID, course.CourseID);
                model.Insert();

                return RedirectToAction("Index", new { userId = CurrentWebContext.CurrentUser.UserID });
            }
            return View(course);
        }

        // GET: Courses/Edit/courseId
        // returns edit view where a teacher can modify the course's data
        public ActionResult Edit(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int id = (int)courseId;

            Course course = CourseHelper.GetById(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            // check if the user trying to access the course is a supervisor
            var supervisors = SupervisorHelper.GetSupervisorsForCourse(id);
            // if he isn't, then return him => denied
            if (!supervisors.Any(x => x.UserID == CurrentWebContext.CurrentUser.UserID))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            // create the viewmodel
            var viewModel = DetailedCourseVM.CreateDetailedCourseVMW(course, supervisors);

            ViewBag.CountOfPendingStudents = ParticipantHelper.GetCountOfPendingParticipants(id);

            return View(viewModel);
        }

        // POST: Courses/Edit/course
        // retrieves the new data for the course and updates it in the database
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

        // GET: Courses/Delete/courseId
        // returns delete view with confirmation of the course being deleted
        public ActionResult Delete(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var id = (int)courseId;
            Course course = CourseHelper.GetById(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            var isSupervisor = SupervisorHelper.IsUserSupervisor(CurrentWebContext.CurrentUser.UserID, id);
            if (!isSupervisor)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ViewBag.Title = course.CourseName;

            var success = course.Delete();
            ViewBag.Success = success;

            return View();
        }

        // GET: Courses/Enroll/userId?courseId
        // returns enroll confirmation view
        public ActionResult Enroll(int userId, int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int id = (int)courseId;
            Course course = CourseHelper.GetById(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            if(CurrentWebContext.CurrentUser.Type == "teacher")
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var participant = new Participant(userId, id, Participant.STATUS_PENDING);
            var success = participant.Insert();

            if (success)
                ViewBag.Message = "Successfully applied for the course, a course supervisor must approve you!";
            else
                ViewBag.Message = "Something went wrong! Please try again.";

            return View();
        }

        // GET: Courses/SeeDetailedOverview/courseId?studentId
        // returns detailed overviev view for the specified student and targeted course
        public ActionResult SeeDetailedOverview(int courseId, int studentId)
        {
            Course course = CourseHelper.GetById(courseId);
            User student = UserHelper.GetById(studentId);

            if (course == null || student == null)
            {
                return HttpNotFound();
            }

            var isSupervisor = SupervisorHelper.IsUserSupervisor(CurrentWebContext.CurrentUser.UserID, courseId);
            if (!isSupervisor)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
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
