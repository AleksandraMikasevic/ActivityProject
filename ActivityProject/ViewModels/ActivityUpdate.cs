using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class ActivityUpdate
    {
        public string StudentID { get; set; }
        public string ProfessorID { get; set; }
        public string ActivityTypeID { get; set; }
        public string CourseID { get; set; }
        public double Points { get; set; }
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public Professor ChosenProfessor { get; set; }
        public Student ChosenStudent { get; set; }
        public ActivityType ActivityType { get; set; }

    }
}
