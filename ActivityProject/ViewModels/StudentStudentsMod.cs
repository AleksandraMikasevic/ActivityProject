using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.ViewModels
{
    public class StudentStudentsMod
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public StudentStudentsMod(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
        public StudentStudentsMod()
        {

        }

    }
}
