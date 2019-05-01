using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Services
{
    public interface IStudentData
    {
        IEnumerable<Student> FindAll();
        Student FindByTranscript(string transc);
        Student Add(Student student);
        Student Find(string id);
        void Delete(string id);
        void Update(Student student);
        Student Add(Student student, List<Course> courses);
        Student Update(Student student, List<Course> courses);

    }
}
