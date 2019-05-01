using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Models
{
    public class ActivityType
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string CourseID { get; set; }
        public double MinPoints { get; set; }
        public double MaxPoints { get; set; }
        public double WeightCoefficient { get; set; }
        public bool Required { get; set; }
        public Course Course { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
