using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Models
{
    public class Person
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
