using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityProject.Data;
using ActivityProject.Models;

namespace ActivityProject.Services
{
    public class SqlProfessorData : IProfessorData
    {
        private ActivityProjectDbContext _context;

        public SqlProfessorData(ActivityProjectDbContext context)
        {
            _context = context;
        }

        public bool CheckProfessor(string username, string password)
        {
            Professor professor = null;
            try
            {
                professor = _context.Professors.Where(n => n.Username == username & n.Password == password).Single();
            }
            catch (Exception ex)
            {
                return false;
            }
            if (professor == null)
                return false;
            return true;
        }

        public Professor Find(string id)
        {
            return _context.Professors.FirstOrDefault(n => n.ID == id);
        }

        public IEnumerable<Professor> FindAll()
        {
            return _context.Professors.OrderBy(n => n.LastName);
        }

        public Professor FindByUsername(string username)
        {
            return _context.Professors.FirstOrDefault(n => n.Username == username);
        }
    }
}
