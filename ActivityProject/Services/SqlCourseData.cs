using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Data;
using ActivityProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityProject.Services
{
    public class SqlCourseData : ICourseData
    {
        private ActivityProjectDbContext _context;

        public SqlCourseData(ActivityProjectDbContext context)
        {
            _context = context;
        }

        public Course Add(Course course)
        {
            _context.Add(course);
            _context.SaveChanges();
            return course;
        }

        public void Delete(string id)
        {
            Course course = _context.Courses.Where(s => s.ID == id).Single();
            IEnumerable<Activity> activities = _context.Activities.Where(a => a.CourseID == course.ID);
            foreach (Activity activity in activities) {
                _context.Activities.Remove(activity);
            }
            _context.Courses.Remove(course);

            _context.SaveChanges();
        }

        public Course Find(string id)
        {
            return _context.Courses.Include(p => p.ActivityTypes).FirstOrDefault(r => r.ID == id);
        }

        public IQueryable<Course> FindAll()
        {
            return _context.Courses;
        }

        public IEnumerable<ActivityType> FindUpdate(string id)
        {
            return _context.ActivityTypes.Where(a => a.CourseID == id);
        }

        public void Update(Course model)
        {
            var existingParent = _context.Courses
                 .Where(p => p.ID == model.ID)
                 .Include(p => p.ActivityTypes).ThenInclude(p => p.Activities)
                 .SingleOrDefault();

            if (existingParent != null)
            {
                // Update parent
                _context.Entry(existingParent).CurrentValues.SetValues(model);

                // Delete children
                foreach (var existingChild in existingParent.ActivityTypes.ToList())
                {
                    if (!model.ActivityTypes.Any(c => c.ID == existingChild.ID))
                    {
                        foreach (Activity ac in existingChild.Activities)
                        {
                            _context.Activities.Remove(ac);
                        }
                        _context.ActivityTypes.Remove(existingChild);
                     
                    }
                }

                // Update and Insert children
                foreach (var childModel in model.ActivityTypes)
                {
                    var existingChild = existingParent.ActivityTypes
                        .Where(c => c.ID == childModel.ID)
                        .SingleOrDefault();

                    if (existingChild != null)
                        // Update child
                        _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                    else
                    {
                        Console.WriteLine("Child");
                        // Insert child
                        var newChild = new ActivityType
                        {
                            ID = childModel.ID,
                            Name = childModel.Name,
                            MinPoints = childModel.MinPoints,
                            MaxPoints = childModel.MaxPoints,
                            WeightCoefficient = childModel.WeightCoefficient,
                            Required = childModel.Required,
                            CourseID = childModel.CourseID
                        };
                        _context.ActivityTypes.Add(newChild);
                        Console.WriteLine("Child");
                    }
                }

                _context.SaveChanges();
            }
        }
    }
}
