using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Data;
using ActivityProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityProject.Services
{
    public class SqlActivityData : IActivityData
    {
        private ActivityProjectDbContext _context;

        public SqlActivityData(ActivityProjectDbContext context)
        {
            _context = context;
        }

        public Activity Add(Activity activity)
        {
            _context.Add(activity);
            _context.SaveChanges();
            return activity;
        }

        public Activity Delete(Activity activity)
        {
            _context.Activities.Update(activity);
            _context.SaveChanges();
            return activity;
        }

        public IEnumerable<Activity> Find(string courseID, string studentID)
        {
            return _context.Activities.Include(p => p.ActivityType)
                .Include(p => p.Professor).Include(p => p.Student).Include(p => p.ActivityType)
                .Where(p => p.Student.ID == studentID).Where(p => p.CourseID == courseID).OrderBy(r => r.CourseID);
        }

        public Activity Find(string studentID, string courseID, string activityTypeID, DateTime date)
        {
            return _context.Activities.Include(a => a.Professor).Include(a => a.Student).Include(a => a.ActivityType).Include(a => a.ActivityType.Course)
                .Where(a => a.StudentID == studentID && a.ActivityTypeID == activityTypeID && a.CourseID == courseID && a.Date == date).FirstOrDefault();
        }

        public IEnumerable<Activity> FindAll()
        {
            return _context.Activities.Include(p => p.ActivityType).Include(p => p.Student).Include(p => p.Professor).Include(p => p.ActivityType.Course);
        }

        public IEnumerable<Activity> FindAllByStudentCourse(string studentID, string courseID)
        {
            return _context.Activities.Include(p => p.ActivityType)
                .Include(p => p.Professor).Include(p => p.Student).Include(p => p.ActivityType)
                .Where(p => p.Student.ID == studentID).Where(p => p.CourseID == courseID).OrderBy(r => r.CourseID);
        }

        public IEnumerable<Activity> FindAllByStudentCourseShow(string studentID, string courseID)
        {
            return _context.Activities.Include(p => p.ActivityType)
    .Include(p => p.Professor).Include(p => p.Student).Include(p => p.ActivityType)
    .Where(p => p.Student.ID == studentID).Where(p => p.CourseID == courseID).Where(a => a.Valid == true).OrderBy(r => r.CourseID);
        }

        public IEnumerable<Activity> FindAllValid()
        {
            return _context.Activities.Include(p => p.ActivityType).Include(p => p.Student).Include(p => p.Professor).Include(p => p.ActivityType.Course).Where(a => a.Valid == true);
        }

        public IEnumerable<Activity> FindSpecific(string studentID, string courseID, string activityTypeID)
        {
            return _context.Activities.Include(p => p.ActivityType)
                .Include(p => p.Professor).Include(p => p.Student).Include(p => p.ActivityType)
                .Where(p => p.Student.ID == studentID).Where(p => p.ActivityTypeID == activityTypeID).Where(p => p.CourseID == courseID).OrderBy(r => r.CourseID);
        }

        public Activity Update(Activity activity)
        {
            _context.Activities.Update(activity);
            _context.SaveChanges();
            return activity;
        }
    }
}
