using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Data;
using ActivityProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityProject.Services
{
    public class SqlCourseStudentData : ICourseStudentData
    {
        private ActivityProjectDbContext _context;

        public SqlCourseStudentData(ActivityProjectDbContext context)
        {
            _context = context;
        }

        public void CalculateGrade(string studentID, string courseID, IEnumerable<Activity> studentsActivity)
        {
            CourseStudent courseStudent = _context.CourseStudents.Include(s => s.Course).Include(s => s.Course.ActivityTypes).Include(s => s.Student).Where(s => s.Course.ID == courseID).Where(s => s.Student.ID == studentID).FirstOrDefault();
            int grade = Calculate(courseStudent, studentsActivity);
            courseStudent.ProposedGrade = grade;
            _context.CourseStudents.Update(courseStudent);
            _context.SaveChanges();

        }

        public int Calculate(CourseStudent courseStudent, IEnumerable<Activity> studentActivities) {
            int finalGrade = 0;
            double points = 0;
            bool passed = true;
            foreach (ActivityType activityType in courseStudent.Course.ActivityTypes)
            {
                bool missed = true;
                foreach (Activity activity in studentActivities)
                {
                    if (activity.ActivityTypeID == activityType.ID && activity.Valid == true)
                    {
                        missed = false;
                        if (activityType.Required == true && activity.Status == false)
                        {
                            return 5;
                        }
                        if (activity.Status == true)
                        {
                            points = points + activity.Points * activityType.WeightCoefficient;
                        }
                    }
                }
                if (missed == true && activityType.Required == true)
                {
                    passed = false;
                }
            }
            if (passed == false)
            {
                return 0;
            }
            if (points <= 50)
            {
                finalGrade = 5;
            }
            if (points > 50 && points <= 60)
            {
                finalGrade = 6;
            }
            if (points > 60 && points <= 70)
            {
                finalGrade = 7;
            }
            if (points > 70 && points <= 80)
            {
                finalGrade = 8;
            }
            if (points > 80 && points <= 90)
            {
                finalGrade = 9;
            }
            if (points >= 91)
            {
                finalGrade = 10;
            }
            return finalGrade;

        }

        public void ChangeFinalGrade(string studentID, string courseID, string grade)
        {
            CourseStudent courseStudent = _context.CourseStudents.Include(s => s.Course).Include(s => s.Course.ActivityTypes).Include(s => s.Student).Where(s => s.Course.ID == courseID).Where(s => s.Student.ID == studentID).FirstOrDefault();
            if (grade == null)
            {
                courseStudent.FinalGrade = null;
                courseStudent.CompletionDate = null;
            }
            else
            {
                courseStudent.FinalGrade = Convert.ToInt32(grade);
                courseStudent.CompletionDate = DateTime.Now;

            }
            _context.CourseStudents.Update(courseStudent);
            _context.SaveChanges();
        }

        public void Delete(CourseStudent courseStudent)
        {
            string courseID = courseStudent.CourseID;
            string studentID = courseStudent.StudentID;
            List<Activity> activities = _context.Activities.Where(a => a.CourseID == courseID && a.StudentID == studentID).ToList();
            foreach (Activity activity in activities)
            {
                _context.Activities.Remove(activity);
            }
            _context.Remove(courseStudent);
            _context.SaveChanges();
        }

        public void FinalizeGrade(string studentID, string courseID)
        {
            Console.WriteLine("Pocelo");
            Console.WriteLine("Finalize grade 1");
            CourseStudent courseStudent = _context.CourseStudents.Include(s => s.Course).Include(s => s.Course.ActivityTypes).Include(s => s.Student).Where(s => s.Course.ID == courseID).Where(s => s.Student.ID == studentID).FirstOrDefault();
            Console.WriteLine("Finalize grade 2");

            courseStudent.FinalGrade = courseStudent.ProposedGrade;
            courseStudent.CompletionDate = DateTime.Now;
            Console.WriteLine("Finalize grade 3");

            _context.CourseStudents.Update(courseStudent);
            _context.SaveChanges();
            Console.WriteLine("Finalize grade 4");

        }

        public CourseStudent Find(string studentID, string courseID)
        {
            return _context.CourseStudents.Include(s => s.Course).Include(s => s.Student).Include(s => s.Course.ActivityTypes).Where(r => r.StudentID == studentID && r.CourseID == courseID).FirstOrDefault();
        }

        public IEnumerable<CourseStudent> FindAll()
        {
            return _context.CourseStudents;
        }

        public IEnumerable<CourseStudent> FindAllBetweenDates(string id, string dateFrom, string dateTo)
        {
            DateTime dateFromDate = DateTime.ParseExact(dateFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime dateToDate = DateTime.ParseExact(dateTo, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            IEnumerable<CourseStudent> courseStudents = _context.CourseStudents.Include(s => s.Student).Where(r => r.CourseID == id && r.CompletionDate != null
           && DateTime.Compare(dateFromDate, (r.CompletionDate ?? DateTime.Now)) <= 0 &&
            DateTime.Compare(dateToDate, (r.CompletionDate ?? DateTime.Now)) >= 0);

            return courseStudents;
        }

        public IEnumerable<CourseStudent> FindAllByCourse(string id)
        {
            return _context.CourseStudents.Include(s => s.Student).Where(r => r.CourseID == id);
        }

        public IEnumerable<CourseStudent> FindAllByStudent(string studentID)
        {
            return _context.CourseStudents.Include(s => s.Course).Where(r => r.StudentID == studentID);
        }

        public void Save(CourseStudent courseStudent)
        {
            _context.Add(courseStudent);
            _context.SaveChanges();
        }
    }
}
