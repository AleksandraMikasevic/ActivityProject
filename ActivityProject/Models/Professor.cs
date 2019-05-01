using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Models
{
    public class Professor : Person
    {
        public string Position { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
