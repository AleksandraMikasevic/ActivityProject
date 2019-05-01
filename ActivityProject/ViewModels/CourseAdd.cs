using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class CourseAdd
    {
        public string CourseID { get; set; }
        public string Name { get; set; }
        public int ESPB { get; set; }
        public List<ActivityType> ActivityType { get; set; }
        public String JsonString { get; set; }
        public CourseAdd()
        {
            ActivityType = new List<ActivityType>();
        }

    }
}
