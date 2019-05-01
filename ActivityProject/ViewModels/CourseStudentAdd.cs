using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class CourseStudentAdd
    {
        public Student Student { get; set; }
        public List<Course> Courses { get; set; }

    }
}
