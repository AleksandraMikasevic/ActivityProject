using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class StudentShowStudent
    {
        public CourseStudent CourseStudent { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public String Professor { get; set; }
        public DateTime Date { get; set; }

    }
}
