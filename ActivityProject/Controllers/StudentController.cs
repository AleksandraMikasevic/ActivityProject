using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Models;
using ActivityProject.Services;
using ActivityProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace ActivityProject.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private IStudentData _studentData;
        private ICourseData _courseData;
        private ICourseStudentData _courseStudentData;
        private IActivityData _activityData;
        private IActivityTypeData _activityTypeData;

        public StudentController(IStudentData studentData, ICourseData courseData, ICourseStudentData courseStudentData, IActivityData activityData, IActivityTypeData activityTypeData)
        {
            _studentData = studentData;
            _courseData = courseData;
            _courseStudentData = courseStudentData;
            _activityData = activityData;
            _activityTypeData = activityTypeData;
        }


        public IActionResult Students()
        {
            Console.WriteLine("Students");
            var model = _studentData.FindAll();
            return View(model);
        }

        [HttpPost]
        [Route("/Student/Students/Table/")]
        public IActionResult ReturnStudents()
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
            var model = _studentData.FindAll();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                Console.WriteLine("SORT COLUMN: " + sortColumn);
                var sortProperty = typeof(Student).GetProperty(sortColumn);
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
                model = model.Where(m => m.TranscriptNumber.StartsWith(searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        public IActionResult StudentsSubjects(string id)
        {
            var model = new StudentStudents ();
            model.Students = _courseStudentData.FindAllByCourse(id);
            model.CourseID = id;
            model.Mod = new List<StudentStudentsMod>();
            model.Mod.Add(new StudentStudentsMod("Passed", "0"));
            model.Mod.Add(new StudentStudentsMod("All students", "1"));
            model.Mod.Add(new StudentStudentsMod("Failed", "2"));

            model.Course = _courseData.Find(id);
            return View(model);
        }

        public IActionResult Add()
        {
            var model = new StudentAdd();
            model.Courses = _courseData.FindAll().ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult AddPost(StudentAdd studentAdd)
        {
            Student student = new Student();
            Console.WriteLine("Dodaje123 studenta123");
            student.FirstName = studentAdd.FirstName;
            student.LastName = studentAdd.LastName;
            student.ID = studentAdd.StudentID;
            student.TranscriptNumber = studentAdd.TranscriptNumber;
            JArray courseArray = JArray.Parse(studentAdd.JsonString);
            List<Course> courses = courseArray.ToObject<List<Course>>();
            _studentData.Add(student, courses);
            return RedirectToAction("Students", "Student");
        }
        [Route("/Student/CoursesStudent/{id}")]
        [HttpPost]
        public IActionResult CoursesStudent(string id)
        {
            var model = _courseStudentData.FindAllByStudent(id);
            Console.WriteLine("ID: ++++++++++++++++++" + id);
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            Console.WriteLine(model.ToList().Count + "------------------------------DUZINA SLUSANJAAA");
            Console.WriteLine("SOrt colummmmmmmmmmmmn --" + sortColumn);
            /* if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
             {
                 var sortProperty = typeof(Slusa).GetProperty(sortColumn);
                 if (sortColumnDirection == "asc")
                 {
                     model = model.OrderBy(p => sortProperty.GetValue(p, null));
                 }
                 if (sortColumnDirection == "desc")
                 {
                     model = model.OrderByDescending(p => sortProperty.GetValue(p, null));
                 }
             }*/

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(m => m.StudentID.StartsWith(searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        [HttpGet]
        public IActionResult ReturnCourseAdd()
        {
            var model = new List<Course>();
            return Json(new { data = model });
        }
        [HttpGet]
        public IActionResult ReturnCoursesUpdate(string id)
        {
            var model = new List<Course>();
            IEnumerable<CourseStudent> courseStudents = new List<CourseStudent>();
            courseStudents = _courseStudentData.FindAllByStudent(id);
            List<Course> courses = new List<Course>();
            foreach (CourseStudent courseStudent in courseStudents)
            {
                courses.Add(courseStudent.Course);
            }
            model = courses;
            return Json(new { data = model });
        }


        [Route("/Student/AddCourseStudent/{CourseID}/{StudentID}")]
        public void AddCourseStudent(string CourseID, string StudentID)
        {
            Console.WriteLine("Dodaj slusanje");
            CourseStudent courseStudent = new CourseStudent();
            courseStudent.CourseID = CourseID;
            courseStudent.StudentID = StudentID;
            courseStudent.ProposedGrade = 0;
            courseStudent.FinalGrade = null;
            courseStudent.EnrollmentDate = DateTime.Now;
            courseStudent.CompletionDate = null;
            _courseStudentData.Save(courseStudent);
        }
        [Route("/Student/DeleteCourseStudent/{CourseID}/{StudentID}")]
        public void DeleteCourseStudent(string CourseID, string StudentID)
        {
            Console.WriteLine("Izbrisi slusanje slusanje");
            CourseStudent courseStudent = _courseStudentData.Find(StudentID, CourseID);
            _courseStudentData.Delete(courseStudent);
        }

        public IActionResult Update(string id)
        {
            var student = _studentData.Find(id);
            var model = new StudentAdd();
            model.FirstName = student.FirstName;
            model.LastName = student.LastName;
            model.StudentID = student.ID;
            model.TranscriptNumber = student.TranscriptNumber;
            IEnumerable<CourseStudent> courseStudents = new List<CourseStudent>();
            courseStudents = _courseStudentData.FindAllByStudent(id);
            List<Course> courses = new List<Course>();
            foreach (CourseStudent courseStudent in courseStudents)
            {
                courses.Add(courseStudent.Course);
            }
            List<Course> allCourses = _courseData.FindAll().ToList();
            List<Course> leftCourses = new List<Course>();
            foreach (Course course in allCourses)
            {
                bool found = false;
                foreach (Course course1 in courses)
                {
                    if (course1.ID == course.ID)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                    leftCourses.Add(course);
            }
            model.Courses = leftCourses;
            return View("Update",model);
        }
        public IActionResult Delete(string id)
        {
            var model = _studentData.Find(id);
            return View("Delete", model);
        }

        [HttpGet]
        [Route("/Student/AddActivity/{id}")]
        public IActionResult AddActivity(string id)
        {
            Course course = _courseData.Find(id);
            //dodas u json
            return Json(new { data = course });
        }


        [HttpPost]
        public IActionResult UpdatePost(StudentAdd studentAdd)
        {
            Student student = new Student();
            student.FirstName = studentAdd.FirstName;
            student.LastName = studentAdd.LastName;
            student.ID = studentAdd.StudentID;
            student.TranscriptNumber = studentAdd.TranscriptNumber;
            JArray coursesArray = JArray.Parse(studentAdd.JsonString);
            List<Course> courses = coursesArray.ToObject<List<Course>>();
            _studentData.Update(student, courses);
            return RedirectToAction("Students", "Student");
        }
        [HttpPost]
        public IActionResult DeletePost(string id)
        {
            _studentData.Delete(id);
            return RedirectToAction("Students", "Student");
        }
        [Route("/Student/ChooseCourse/{id}")]
        public IActionResult ChooseCourse(string id)
        {
            Student student = _studentData.Find(id);
            IEnumerable<CourseStudent> courseStudents = new List<CourseStudent>();
            courseStudents = _courseStudentData.FindAllByStudent(id);
            List<Course> courses = new List<Course>();
            foreach (CourseStudent courseStudent in courseStudents)
            {
                courses.Add(courseStudent.Course);
            }
            List<Course> allCourses = _courseData.FindAll().ToList();
            List<Course> leftCourses = new List<Course>();

            foreach (Course course in allCourses)
            {
                bool found = false;
                foreach (Course course1 in courses)
                {
                    if (course1.ID == course.ID)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                    leftCourses.Add(course);
            }
            var model = new CourseStudentAdd();
            model.Courses = leftCourses;
            model.Student = student;
            return View(model);
        }


        public IActionResult LeftCourses(string id)
        {
            Console.WriteLine("Vrati preostale predmete");

            IEnumerable<CourseStudent> courseStudents = new List<CourseStudent>();
            courseStudents = _courseStudentData.FindAllByStudent(id);
            List<Course> courses = new List<Course>();
            foreach (CourseStudent courseStudent in courseStudents)
            {
                courses.Add(courseStudent.Course);
            }
            List<Course> allCourses = _courseData.FindAll().ToList();
            List<Course> leftCourses = new List<Course>();

            foreach (Course course in allCourses)
            {
                bool found = false;
                foreach (Course course1 in courses)
                {
                    if (course1.ID == course.ID)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                    leftCourses.Add(course);
            }
            SelectList coursesSelect = new SelectList(leftCourses, "ID", "Name", 0);
            return Json(coursesSelect);
        }
        [Route("/Student/StudentsByCourse/{Id}/{DateFrom}/{DateTo}/{Mod}")]
        [HttpPost]
        public IActionResult StudentsByCourse(string Id, string DateFrom, string DateTo, string Mod)
        {
            Console.WriteLine("VRACAAAAAAAAAAA STUDENTE");
            Console.WriteLine("ID: ++++++++++++++++++" + Id);
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var model = _courseStudentData.FindAllByCourse(Id).Where(s => s.FinalGrade != null);
            if (Mod == "1")
            {
                Console.WriteLine("Svi studenti");
                model = _courseStudentData.FindAllByCourse(Id);
            }
            if (Mod == "2")
            {
                model = _courseStudentData.FindAllByCourse(Id).Where(s => s.FinalGrade == null);
            }
            if (Mod == "0")
            {
                Console.WriteLine(DateFrom + " datumOd");
                Console.WriteLine(DateTo + " datumDo");
                if (DateFrom != "undefined" && DateTo != "undefined")
                {
                    Console.WriteLine("uslo u undefined");
                    model = _courseStudentData.FindAllBetweenDates(Id, DateFrom, DateTo);
                }
            }
            Console.WriteLine(model.ToList().Count + "------------------------------DUZINA SLUSANJAAA");
            Console.WriteLine("SOrt colummmmmmmmmmmmn --" + sortColumn);
            /* if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
             {
                 var sortProperty = typeof(Slusa).GetProperty(sortColumn);
                 if (sortColumnDirection == "asc")
                 {
                     model = model.OrderBy(p => sortProperty.GetValue(p, null));
                 }
                 if (sortColumnDirection == "desc")
                 {
                     model = model.OrderByDescending(p => sortProperty.GetValue(p, null));
                 }
             }*/

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(m => m.StudentID.StartsWith(searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [Route("/Student/CoursesByStudent/{id}")]
        [HttpPost]
        public IActionResult CoursesByStudent(string id)
        {
            var model = _courseStudentData.FindAllByStudent(id);
            Console.WriteLine(model.ToList().Count + "------------------------------DUZINA SLUSANJAAA");
            Console.WriteLine("SLusanja duzinaaaaaaaaaa: " + model.ToList().Count);
            Console.WriteLine(Json(new { recordsFiltered = model.ToList(), recordsTotal = model.ToList(), data = model.ToList() }).Value);
            string o = Newtonsoft.Json.JsonConvert.SerializeObject(Json(new { recordsFiltered = model.ToList(), recordsTotal = model.ToList(), data = model.ToList() }));
            Console.WriteLine("p ----------- " + o);
            return Json(new { recordsFiltered = model.ToList(), recordsTotal = model.ToList(), data = model.ToList() });
        }


        [Route("/Student/StudentCourseActivities/{StudentID}/{CourseID}")]
        public IActionResult StudentCourseActivities(string StudentID, string CourseID)
        {
            StudentShowStudent model = new StudentShowStudent();
            model.CourseStudent = _courseStudentData.Find(StudentID, CourseID);
            model.Activities = _activityData.FindAllByStudentCourseShow(StudentID, CourseID);
            return View("ShowStudent", model);
        }
        [Route("/Student/PDF/{StudentID}/{CourseID}")]
        public IActionResult PDF(string StudentID, string CourseID)
        {
            StudentShowStudent model = new StudentShowStudent();
            model.CourseStudent = _courseStudentData.Find(StudentID, CourseID);
            model.Professor = HttpContext.Session.GetString("professor");
            model.Date = DateTime.Now;
            model.Activities = _activityData.FindAllByStudentCourse(StudentID, CourseID);
            //return new ViewAsPdf("PrikazStudentaPDF", model) { PageOrientation = Orientation.Landscape, CustomSwitches = "--viewport-size 1000x1000" };
            return new ViewAsPdf("ShowStudentPDF", model)
            {
                PageOrientation = Orientation.Landscape,
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };

        }
        [Route("/Student/FinalizeGrade/{StudentID}/{CourseID}")]
        public IActionResult FinalizeGrade(string StudentID, string CourseID)
        {
            Console.WriteLine("finalize grade controller 1");
            _courseStudentData.FinalizeGrade(StudentID, CourseID);
            Console.WriteLine("finalize grade controller 2");
            return RedirectToAction("StudentCourseActivities");
        }
        [Route("/Student/DeleteFinalGrade/{StudentID}/{CourseID}")]
        public IActionResult DeleteFinalGrade(string StudentID, string CourseID)
        {
            _courseStudentData.ChangeFinalGrade(StudentID, CourseID, null);
            return RedirectToAction("StudentCourseActivities");
        }

        [Route("/Student/UpdateFinalGrade/{StudentID}/{CourseID}")]
        [HttpPost]
        public IActionResult UpdateFinalGrade(string StudentID, string CourseID, string Grade)
        {
            _courseStudentData.ChangeFinalGrade(StudentID, CourseID, Grade);
            return RedirectToAction("StudentCourseActivities");
        }


    }
}