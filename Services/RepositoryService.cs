using SOLID_Example.Interfaces;
using SOLID_Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Example.Services
{
    public class RepositoryService : ICrud<Repository>
    {
        private IDatabase<Repository> _database;

        public RepositoryService(IDatabase<Repository> database)
        {
            _database = database;
        }

        public void Add(Repository repository)
        {
            _database.Add(repository);
        }

        public void Update(Repository repository)
        {
            _database.Update(repository);
        }

        public void Delete(Repository repository)
        {
            _database.Delete(repository);
        }

        public List<Repository> GetAll()
        {
            return _database.GetAll().ToList();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
