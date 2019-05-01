using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Data;
using ActivityProject.Models;

namespace ActivityProject.Services
{
    public class SqlStudentData : IStudentData
    {
        private ActivityProjectDbContext _context;

        public SqlStudentData(ActivityProjectDbContext context)
        {
            _context = context;
        }

        public Student Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student Add(Student student, List<Course> courses)
        {
            _context.Students.Add(student);
            foreach (Course course in courses)
            {
                CourseStudent courseStudent = new CourseStudent();
                courseStudent.StudentID = student.ID;
                courseStudent.CourseID = course.ID;
                courseStudent.FinalGrade = null;
                courseStudent.CompletionDate = null;
                courseStudent.EnrollmentDate = DateTime.Now;
                courseStudent.ProposedGrade = 0;
                _context.Add(courseStudent);
            }
            _context.SaveChanges();
            return student;
        }

        public void Delete(string id)
        {
            Student student = _context.Students.Where(s => s.ID == id).Single();
            var model = _context.Activities.Where(a => a.StudentID == id);
            foreach (Activity activity in model)
            {
                _context.Remove(activity);
            }
            _context.Students.Remove(student);
            _context.SaveChanges();
        }

        public Student Find(string id)
        {
            return _context.Students.FirstOrDefault(r => r.ID == id);
        }

        public IEnumerable<Student> FindAll()
        {
            return _context.Students.OrderBy(r => r.TranscriptNumber);
        }

        public Student FindByTranscript(string transc)
        {
            return _context.Students.FirstOrDefault(r => r.TranscriptNumber == transc);
        }

        public void Update(Student student)
        {
            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public Student Update(Student student, List<Course> courses)
        {
            _context.Students.Update(student);

            foreach (CourseStudent courseStudent1 in _context.CourseStudents.Where(s => s.StudentID == student.ID))
            {
                bool found = false;
                foreach (Course course in courses)
                {
                    if (courseStudent1.CourseID == course.ID)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    _context.CourseStudents.Remove(courseStudent1);
                }
            }

            foreach (Course course in courses)
            {
                bool found = false;
                foreach (CourseStudent courseStudent1 in _context.CourseStudents)
                {
                    if (courseStudent1.StudentID == student.ID && courseStudent1.CourseID == course.ID)
                    {
                        found = true;
                        break;
                    }

                }
                if (found == false)
                {
                    CourseStudent courseStudent = new CourseStudent();
                    courseStudent.StudentID = student.ID;
                    courseStudent.CourseID = course.ID;
                    courseStudent.FinalGrade = null;
                    courseStudent.CompletionDate = null;
                    courseStudent.EnrollmentDate = DateTime.Now;
                    courseStudent.ProposedGrade = 0;
                    _context.Add(courseStudent);
                }

            }
            _context.SaveChanges();
            return student;
        }
    }
}
