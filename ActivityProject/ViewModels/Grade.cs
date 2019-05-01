using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class Grade
    {
        public int ProposedGrade { get; set; }
        public int StudentNumber { get; set; }
        public Grade(int grade, int studentNumber)
        {
            this.ProposedGrade = grade;
            this.StudentNumber = studentNumber;
        }

    }
}
