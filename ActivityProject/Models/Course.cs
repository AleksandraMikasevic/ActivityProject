using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Models
{
    public class Course
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int ESPB { get; set; }
        [JsonIgnore]
        public IEnumerable<ActivityType> ActivityTypes { get; set; }
    }
}
