using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Services
{
    public interface IActivityData
    {
        IEnumerable<Activity> FindAllByStudentCourse(string studentID, string courseID);
        Activity Add(Activity activity);
        Activity Update(Activity activity);
        Activity Delete(Activity activity);
        IEnumerable<Activity> Find(string courseID, string studentID);
        IEnumerable<Activity> FindAll();
        IEnumerable<Activity> FindSpecific(string studentID, string courseID, string activityTypeID);
        IEnumerable<Activity> FindAllByStudentCourseShow(string studentID, string courseID);
        IEnumerable<Activity> FindAllValid();
        Activity Find(string studentID, string courseID, string activityTypeID, DateTime date);

    }
}
