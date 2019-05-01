using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Models
{
    public class CourseStudent
    {
        public DateTime EnrollmentDate { get; set; }
        public Nullable<DateTime> CompletionDate { get; set; }
        public Nullable<int> ProposedGrade { get; set; }
        public Nullable<int> FinalGrade { get; set; }
        public string StudentID { get; set; }
        public string CourseID { get; set; }
        public Student Student { get; set; }
        public Course  Course { get; set; }

    }
}
