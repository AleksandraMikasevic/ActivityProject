using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class StudentAdd
    {
        public string StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TranscriptNumber { get; set; }
        public List<Course> Courses { get; set; }
        public String JsonString { get; set; }

    }
}
