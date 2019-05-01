using ActivityProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityProject.Services
{
    public interface IProfessorData
    {
        IEnumerable<Professor> FindAll();
        Professor Find(string id);
        bool CheckProfessor(string username, string password);
        Professor FindByUsername(string username);

    }
}
