using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class CourseGraphView
    {
        public List<Grade> Grades { get; set; }
        public Course Course { get; set; }
    }
}
