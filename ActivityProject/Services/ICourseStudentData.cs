using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Services
{
    public interface ICourseStudentData
    {
        IEnumerable<CourseStudent> FindAll();
        IEnumerable<CourseStudent> FindAllByStudent(string studentID);
        IEnumerable<CourseStudent> FindAllByCourse(string id);
        CourseStudent Find(string studentID, string courseID);
        void CalculateGrade (string studentID, string courseID, IEnumerable<Activity> studentsActivity);
        void FinalizeGrade(string studentID, string courseID);
        void Save(CourseStudent courseStudent);
        void Delete(CourseStudent courseStudent);
        void ChangeFinalGrade(string studentID, string courseID, string grade);
        IEnumerable<CourseStudent> FindAllBetweenDates(string id, string dateFrom, string dateTo);

    }
}
