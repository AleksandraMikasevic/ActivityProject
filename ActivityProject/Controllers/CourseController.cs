using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Models;
using ActivityProject.Services;
using ActivityProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;

namespace ActivityProject.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private ICourseData _courseData;
        private ICourseStudentData _courseStudentData;

        public CourseController(ICourseData courseData, ICourseStudentData courseStudentData)
        {
            _courseData = courseData;
            _courseStudentData = courseStudentData;
        }

        public IActionResult Courses()
        {
            var model = _courseData.FindAll();
            return View(model);
        }
        [HttpPost]
        [Route("/Course/Table")]
        public IActionResult ReturnCourses()
        {
            Console.WriteLine("vraca kurseva");
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var model = _courseData.FindAll();
            Console.WriteLine("vraca kurseva 1");

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                var sortProperty = typeof(Course).GetProperty(sortColumn);
                if (sortColumnDirection == "asc")
                {
                    model = model.OrderBy(p => sortProperty.GetValue(p, null));
                }
                if (sortColumnDirection == "desc")
                {
                    model = model.OrderByDescending(p => sortProperty.GetValue(p, null));
                }
            }
            Console.WriteLine("vraca kurseva 2");

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(m => m.Name.StartsWith(searchValue, true, null));
            }
            Console.WriteLine("vraca kurseva 3");

            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            Console.WriteLine("vraca kurseva 4");

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        public IActionResult ShowCourse(string id)
        {

            Course course = _courseData.Find(id);
            return View(course);
        }

        [HttpPost]
        [Route("/Course/ActivityTypes/Table/{id}")]
        public IActionResult ReturnActivityTypes(string id)
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
            var model = _courseData.Find(id).ActivityTypes;
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                var sortProperty = typeof(ActivityType).GetProperty(sortColumn);
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
                Console.WriteLine("SEARCH VALUEEE--------------------------------------" + searchValue);
                model = model.Where(m => m.Name.StartsWith(
                    searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        public IActionResult Add()
        {
            CourseAdd course = new CourseAdd();
            return View(course);
        }
        public IActionResult Update(string id)
        {
            CourseAdd courseAdd = new CourseAdd();
            Course course = _courseData.Find(id);
            courseAdd.CourseID = course.ID;
            courseAdd.Name = course.Name;
            courseAdd.ActivityType = course.ActivityTypes.ToList();
            courseAdd.ESPB = course.ESPB;
            ////////////////////////////////////////why is this here
            foreach (ActivityType activityType in courseAdd.ActivityType)
            {
                activityType.Course = null;
            }
            return View(courseAdd);
        }
        public IActionResult Delete(string id)
        {
            CourseAdd courseAdd = new CourseAdd();
            Course course = _courseData.Find(id);
            courseAdd.CourseID = course.ID;
            courseAdd.Name = course.Name;
            courseAdd.ActivityType = course.ActivityTypes.ToList();
            courseAdd.ESPB = course.ESPB;
            return View(courseAdd);
        }
        [HttpGet]
        public IActionResult ActivityTypesForAdd()
        {
            var model = new List<ActivityTypeAdd>();
            return Json(new { data = model });
        }


        [HttpGet]
        [Route("/Course/ActivityTypesForUpdate/{id}")]
        public IActionResult ActivityTypesForUpdate(string id)
        {
            var data = _courseData.FindUpdate(id).ToList();
            //////////////////////why is this here
            foreach (ActivityType activityType in data)
            {
                activityType.Course = null;
            }
            return Json(new { data = data });
        }
        [HttpGet]
        [Route("/Course/AddType/{Name}/{MinPoints}/{MaxPoints}/{WeightCoefficient}/{Required}")]
        public IActionResult AddType(string Name, string MinPoints, string MaxPoints, string WeightCoefficient, string Required)
        {
            ActivityType activityType = new ActivityType();
            /////////////////////is it okay 
            activityType.ID = Guid.NewGuid().ToString();
            activityType.Name = Name;
            Console.WriteLine("wc: "+WeightCoefficient);
            activityType.MinPoints = Double.Parse(MinPoints);
            activityType.MaxPoints = Double.Parse(MaxPoints);
            activityType.WeightCoefficient = Convert.ToDouble(WeightCoefficient);
            Console.WriteLine("wc: " + activityType.WeightCoefficient);

            activityType.Required = Boolean.Parse(Required);
            //dodas u json
            return Json(new { data = activityType });
        }
        //sandra sandra
        [HttpGet]
        [Route("/Course/UpdateType/{TypeID}/{Name}/{MinPoints}/{MaxPoints}/{WeightCoefficient}/{Required}")]
        public IActionResult UpdateType(string TypeID, string Name, string MinPoints, string MaxPoints, string WeightCoefficient, string Required)
        {
            ActivityType activityType = new ActivityType();
            /////////////////////is it okay 
            activityType.ID = TypeID;
            activityType.Name = Name;
            activityType.MinPoints = Double.Parse(MinPoints);
            activityType.MaxPoints = Double.Parse(MaxPoints);
            activityType.WeightCoefficient = Double.Parse(WeightCoefficient);
            activityType.Required = Boolean.Parse(Required);
        
            return Json(new { data = activityType });
        }
        [HttpGet]
        public IActionResult DeleteType(string id)
        {
            ActivityType activityType = new ActivityType();
            //pronadjes tip aktivnosti iz jsona i i izbrises onog sa tom sifrom
            return Json(new { data = activityType });
        }

        [HttpPost]
        public IActionResult AddPost(CourseAdd courseAdd)
        {
            Course course = new Course();
            course.Name = courseAdd.Name;
            course.ID = courseAdd.CourseID;
            course.ESPB = courseAdd.ESPB;
            JArray typesArray = JArray.Parse(courseAdd.JsonString);
            course.ActivityTypes = typesArray.ToObject<List<ActivityType>>();
            _courseData.Add(course);
            return RedirectToAction("Courses", "Course");
        }
        [HttpPost]
        public IActionResult UpdatePost(CourseAdd courseAdd)
        {
            Course course = new Course();
            course.Name = courseAdd.Name;
            course.ID = courseAdd.CourseID;
            course.ESPB = courseAdd.ESPB;
            JArray typesArray = JArray.Parse(courseAdd.JsonString);
            course.ActivityTypes = typesArray.ToObject<List<ActivityType>>();
            foreach (ActivityType type in course.ActivityTypes)
            {
                type.CourseID = course.ID;
                type.Course = course;
            }
            _courseData.Update(course);
            return RedirectToAction("Courses", "Course");
        }
        [Route("/Course/DeletePost/{id}")]
        [HttpPost]
        public IActionResult DeletePost(string id)
        {
            _courseData.Delete(id);
            return RedirectToAction("Courses", "Course");
        }
        [HttpPost]
        public IActionResult ReturnActivityTypesForCB(string id)
        {
            IEnumerable<ActivityType> activityTypes = new List<ActivityType>();
            if (!id.Equals("0"))
            {
                activityTypes = _courseData.Find(id).ActivityTypes;
            }
            SelectList activitiesSelect = new SelectList(activityTypes, "ID", "Name", 0);
            return Json(activitiesSelect);
        }
        [HttpPost]
        public IActionResult ReturnStudentsForCB(string id)
        {
            IEnumerable<CourseStudent> courseStudents = new List<CourseStudent>();
            courseStudents = _courseStudentData.FindAllByCourse(id).Where(s => s.FinalGrade == null);
            List<Student> students = new List<Student>();
            foreach (CourseStudent courseStudent in courseStudents)
            {
                students.Add(courseStudent.Student);
            }
            SelectList studentsSel = new SelectList(students.Select(s => new { Text = s.FirstName + " " + s.LastName + "-" + s.TranscriptNumber, ID = s.ID }), "ID", "Text", 0);
            return Json(studentsSel);
        }

        public IActionResult GraphsView(string id)
        {
            IEnumerable<CourseStudent> courseStudents = _courseStudentData.FindAll();
            courseStudents = courseStudents.Where(s => s.FinalGrade != null || s.ProposedGrade == 5).Where(s => s.CourseID == id);
            List<Grade> grades = new List<Grade>();
            grades = grades.ToList();
            grades.Add(new Grade(5, courseStudents.Count(s => s.ProposedGrade == 5)));
            grades.Add(new Grade(6, courseStudents.Count(s => s.ProposedGrade == 6)));
            grades.Add(new Grade(7, courseStudents.Count(s => s.ProposedGrade == 7)));
            grades.Add(new Grade(8, courseStudents.Count(s => s.ProposedGrade == 8)));
            grades.Add(new Grade(9, courseStudents.Count(s => s.ProposedGrade == 9)));
            grades.Add(new Grade(10, courseStudents.Count(s => s.ProposedGrade == 10)));
            CourseGraphView courseGraphView = new CourseGraphView();
            courseGraphView.Course = _courseData.Find(id);
            courseGraphView.Grades = grades;
            var model = courseGraphView;
            return View(model);
        }

   

    }
}