using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Data;
using ActivityProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityProject.Services
{
    public class SqlActivityTypeData : IActivityTypeData
    {
        private ActivityProjectDbContext _context;

        public SqlActivityTypeData(ActivityProjectDbContext context)
        {
            _context = context;
        }

        public ActivityType Find(string courseID, string activityTypeID)
        {
            return _context.ActivityTypes.Include(ta => ta.Course).FirstOrDefault(ta => ta.CourseID == courseID && ta.ID == activityTypeID);
        }

        public IEnumerable<ActivityType> FindAll()
        {
            return _context.ActivityTypes.Include(a => a.Course);
        }

        public IEnumerable<ActivityType> FindAllByCourse(string courseID)
        {
            return _context.ActivityTypes.Include(a => a.Course).Where(a => a.CourseID == courseID).OrderBy(r => r.CourseID);
        }
    }
}
