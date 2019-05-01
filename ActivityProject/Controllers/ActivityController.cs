using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Models;
using ActivityProject.Services;
using ActivityProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityProject.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private IActivityData _activityData;
        private IActivityTypeData _activityTypeData;
        private ICourseStudentData _courseStudentData;
        private IStudentData _studentData;
        private ICourseData _courseData;
        private IProfessorData _professorData;

        public ActivityController(IActivityData activityData, IActivityTypeData activityTypeData, ICourseStudentData courseStudentData, IStudentData studentData, ICourseData courseData, IProfessorData professorData)
        {
            _activityData = activityData;
            _activityTypeData = activityTypeData;
            _courseStudentData = courseStudentData;
            _studentData = studentData;
            _courseData = courseData;
            _professorData = professorData;
        }

        [Route("/Activity/Add/{courseID}/{activityTypeID}/{professorID}/{studentID}")]
        [HttpGet]
        public IActionResult Add(string courseID, string activityTypeID, string professorID, string studentID)
        {
            ActivityAdd model = new ActivityAdd();
            model.ActivityTypeID = activityTypeID;
            model.CourseID = courseID;
            model.StudentID = studentID;
            model.ProfessorID = professorID;
            model.Professors = _professorData.FindAll();
            if (studentID == null || studentID == "null")
            {
                model.Courses = _courseData.FindAll();
            }
            else
            {
                model.Courses = CoursesForCB(studentID);
            }
            if (courseID != "undefined")
            {
                model.ActivityTypes = _courseData.Find(courseID).ActivityTypes;
            }
            else
            {
                model.ActivityTypes = _activityTypeData.FindAll();
            }
            if (courseID == null || courseID == "null")
            {
                model.Students = _studentData.FindAll();
            }
            else
            {
                model.Students = StudentsForCB(courseID);

            }
            return View(model);
        }

        [Route("/Activity/AddPost")]
        [HttpPost]
        public IActionResult AddPost(ActivityAdd model)
        {

            Activity activity = new Activity();
            activity.StudentID = model.StudentID;
            activity.ProfessorID = model.ProfessorID;
            activity.ActivityTypeID = model.ActivityTypeID;
            activity.CourseID = model.CourseID;
            activity.Points = model.Points;

            activity.Date = model.Date;
            activity.ActivityType = _activityTypeData.Find(model.CourseID, model.ActivityTypeID);
            List<Activity> activities = _activityData.FindSpecific(model.StudentID, model.CourseID, model.ActivityTypeID).ToList();
            foreach (Activity activity1 in activities)
            {
                activity1.Valid = false;
                _activityData.Delete(activity1);
            }
            if (activity.ActivityType.Required == true)
            {
                Console.WriteLine("obavezna je true");
                if (model.Points >= activity.ActivityType.MaxPoints * 0.5)
                {
                    Console.WriteLine("br poena true");
                    activity.Status = true;
                }
                else
                {
                    Console.WriteLine("br poena false");
                    activity.Status = false;
                }
            }
            else
            {
                activity.Status = true;
            }
            activity.Valid = true;
            _activityData.Add(activity);
            _courseStudentData.CalculateGrade(model.StudentID, model.CourseID, _activityData.FindAllByStudentCourse(model.StudentID, model.CourseID));
            return RedirectToAction("Courses", "Course");
        }

        [Route("/Activity/ActivitiesByStudent/{courseID}/{studentID}")]
        public IActionResult ActivitiesByStudent(string courseID, string studentID)
        {
            ActivityStudent model = new ActivityStudent();
            model.StudentID = studentID;
            model.CourseID = courseID;
            return View("ActivityStudents", model);
        }

        [Route("/Activity/Activities/Table/{CourseID}/{StudentID}")]
        [HttpPost]
        public IActionResult ReturnActivities(string CourseID, string StudentID)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var model = _activityData.Find(CourseID, StudentID);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                var sortProperty = typeof(Activity).GetProperty(sortColumn);
                if (sortColumnDirection == "asc")
                {
                    model = model.OrderBy(p => sortProperty.GetValue(p, null));
                }
                if (sortColumnDirection == "desc")
                {
                    model = model.OrderByDescending(p => sortProperty.GetValue(p, null));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(m => m.Student.ID.StartsWith(
                    searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        [Route("/Activity/Activities/")]
        public IActionResult Activities()
        {
            var model = _activityData.FindAll();
            List<Activity> activities = model.ToList();

            for (int i = 0; i < activities.Count; i++)
            {
                Activity activity = activities.ElementAt(i);
                CourseStudent courseStudent = _courseStudentData.Find(activity.StudentID, activity.CourseID);

                if (courseStudent.FinalGrade != null)
                {
                    activities.Remove(activity);
                }
            }
            model = activities;
            return View(model);
        }

        [Route("/Activity/ActivitiesPost/")]
        [HttpPost]
        public IActionResult ActivitiesPost()
        {

            var model = _activityData.FindAll().Where(a => a.Valid == true);
            Console.WriteLine("ovo pravi problem");

            foreach (Activity activity in model)
            {

                CourseStudent courseStudent = _courseStudentData.Find(activity.StudentID, activity.CourseID);
                if (courseStudent.FinalGrade != null)
                {
                    model = model.Where(m => m.StudentID != activity.StudentID || m.ProfessorID != activity.ProfessorID ||
                    m.Date != activity.Date ||
                    m.CourseID != activity.CourseID ||
                    m.ActivityTypeID != activity.ActivityTypeID);
                }


            }
            var data = model.ToList();
            return Json(new { data = data });
        }

        [Route("/Activity/Update/{StudentID}/{CourseID}/{ActivityTypeID}/{Date}/{ProfessorID}")]
        public IActionResult Update()
        {

            var model = _activityData.FindAll();
            return View(model);
        }

        public List<Course> CoursesForCB(string id)
        {   
            IEnumerable<CourseStudent> courseStudents = new List<CourseStudent>();
            courseStudents = _courseStudentData.FindAllByStudent(id);
            List<Course> courses = new List<Course>();
            foreach (CourseStudent courseStudent in courseStudents)
            {
                courses.Add(courseStudent.Course);
            }
            return courses;
        }
        public List<Student> StudentsForCB(string id)
        {
            IEnumerable<CourseStudent> courseStudents = new List<CourseStudent>();
            courseStudents = _courseStudentData.FindAllByCourse(id);
            List<Student> students = new List<Student>();
            foreach (CourseStudent courseStudent in courseStudents)
            {
                students.Add(courseStudent.Student);
            }

            return students;
        }


        [Route("/Activity/Update/{professorID}/{studentID}/{courseID}/{activityTypeID}/{date}/{points}")]
        [HttpGet]
        public IActionResult Update(string professorID, string studentID, string courseID, string activityTypeID, string date, string points)
        {
            ActivityUpdate model = new ActivityUpdate();
            model.ActivityTypeID = activityTypeID;
            model.CourseID = courseID;
            model.StudentID = studentID;
            model.ProfessorID = professorID;
            model.ChosenProfessor = _professorData.Find(professorID);
            model.ChosenStudent = _studentData.Find(studentID);
            model.ActivityType = _activityTypeData.Find(courseID, activityTypeID);
            model.Date = DateTime.Parse(date);
            model.Points = Int32.Parse(points);
            return View("Update", model);
        }

        [Route("/Activity/UpdatePost")]
        [HttpPost]
        public IActionResult UpdatePost(ActivityUpdate model)
        {
            Activity activity = new Activity();
            activity.StudentID = model.StudentID;
            activity.ProfessorID = model.ProfessorID;
            activity.ActivityTypeID = model.ActivityTypeID;
            activity.CourseID = model.CourseID;
            activity.Points = model.Points;
            activity.Date = model.Date;
            activity.ActivityType = _activityTypeData.Find(model.CourseID, model.ActivityTypeID);
            if (activity.ActivityType.Required == true)
            {
                if (model.Points >= activity.ActivityType.MaxPoints * 0.5)
                    activity.Status = true;
                else
                    activity.Status = false;
            }
            else
            {
                activity.Status = true;
            }
            activity.Valid = true;
            _activityData.Update(activity);
            _courseStudentData.CalculateGrade(model.StudentID, model.CourseID, _activityData.FindAllByStudentCourse(model.StudentID, model.CourseID));
            return RedirectToAction("Activities");
        }

        [Route("/Activity/Delete/{professorID}/{studentID}/{courseID}/{activityTypeID}/{date}/{points}")]
        [HttpGet]
        public IActionResult Delete(string professorID, string studentID, string courseID, string activityTypeID, string date, string points)
        {
            ActivityUpdate model = new ActivityUpdate();
            model.ActivityTypeID = activityTypeID;
            model.CourseID = courseID;
            model.StudentID = studentID;
            model.ProfessorID = professorID;
            model.ChosenProfessor = _professorData.Find(professorID);
            model.ChosenStudent = _studentData.Find(studentID);
            model.ActivityType = _activityTypeData.Find(courseID, activityTypeID);
            model.Date = DateTime.Parse(date);
            model.Points = Int32.Parse(points);
            return View("Delete", model);
        }

        [Route("/Activity/DeletePost")]
        [HttpPost]
        public IActionResult DeletePost(ActivityUpdate model)
        {
            Activity activity = new Activity();
            activity.StudentID = model.StudentID;
            activity.ProfessorID = model.ProfessorID;
            activity.ActivityTypeID = model.ActivityTypeID;
            activity.CourseID = model.CourseID;
            activity.Points = model.Points;
            activity.Date = model.Date;
            activity.ActivityType = _activityTypeData.Find(model.CourseID, model.ActivityTypeID);
            if (activity.ActivityType.Required == true)
            {
                if (model.Points >= activity.ActivityType.MaxPoints * 0.5)
                    activity.Status = true;
                else
                    activity.Status = false;
            }
            else
            {
                activity.Status = true;
            }
            activity.Valid = false;
            _activityData.Delete(activity);
            _courseStudentData.CalculateGrade(model.StudentID, model.CourseID, _activityData.FindAllByStudentCourse(model.StudentID, model.CourseID));
            return RedirectToAction("Activities");
        }
        [Route("/Activity/Show/{studentID}/{courseID}/{activityTypeID}/{date}")]
        [HttpGet]
        public IActionResult Show(string studentID, string courseID, string activityTypeID, string date)
        {
            DateTime date1 = DateTime.Parse(date);
            var model = _activityData.Find(studentID, courseID, activityTypeID, date1);
            return View("ShowActivity", model);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}