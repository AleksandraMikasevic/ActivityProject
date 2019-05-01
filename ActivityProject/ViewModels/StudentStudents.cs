using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class StudentStudents
    {
        public IEnumerable<CourseStudent> Students { get; set; }
        public string CourseID { get; set; }
        public Course Course { get; set; }
        public List<StudentStudentsMod> Mod { get; set; }

    }
}
