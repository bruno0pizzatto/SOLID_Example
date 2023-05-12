using SOLID_Example.Interfaces;
using SOLID_Example.Models;

namespace SOLID_Example.Services
{
    public class UserService : ICrud<User>
    {
        private IDatabase<User> _database;

        public UserService(IDatabase<User> database)
        {
            _database = database;
        }

        public void Add(User user)
        {
            _database.Add(user);
        }

        public void Update(User user)
        {
            _database.Update(user);
        }

        public List<User> GetAll()
        {
            return _database.GetElements().ToList();
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public void Delete(User user)
        {
            _database.Delete(user);
        }
    }
}
