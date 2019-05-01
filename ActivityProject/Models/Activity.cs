using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Models
{
    public class Activity
    {
        public DateTime Date { get; set; }
        public string ProfessorID { get; set; }
        public string StudentID { get; set; }
        public string ActivityTypeID { get; set; }
        public string CourseID { get; set; }
        public double Points { get; set; }
        public bool Status { get; set; }
        public bool Valid { get; set; }
        public Professor Professor { get; set; }
        public Student Student { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}
