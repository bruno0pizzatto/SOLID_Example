
namespace SOLID_Example.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public User() { }
        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"Name: {Name} | Email: {Email} | Id: {Id}"; ;
        }
    }
}
