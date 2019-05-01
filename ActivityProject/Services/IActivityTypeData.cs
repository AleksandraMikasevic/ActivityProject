using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Services
{
    public interface IActivityTypeData
    {
        IEnumerable<ActivityType> FindAllByCourse(string courseID);
        IEnumerable<ActivityType> FindAll();
        ActivityType Find(string courseID, string activityTypeID);

    }
}
