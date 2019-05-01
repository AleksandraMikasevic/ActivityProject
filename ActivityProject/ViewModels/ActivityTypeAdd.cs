using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class ActivityTypeAdd
    {
        public string Name { get; set; }
        public string ActivityTypeID { get; set; }
        public double MinPoints { get; set; }
        public double MaxPoints { get; set; }
        public double WeightCoefficient { get; set; }
        public bool Required { get; set; }

    }
}
