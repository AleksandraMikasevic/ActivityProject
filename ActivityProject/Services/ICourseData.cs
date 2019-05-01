using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Services
{
    public interface ICourseData
    {
        IQueryable<Course> FindAll();
        Course Find(string id);
        Course Add(Course course);
        IEnumerable<ActivityType> FindUpdate(string id);
        void Update(Course course);
        void Delete(string id);

    }
}
