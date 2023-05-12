
namespace SOLID_Example.Models
{
    public class Repository : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Repository() { }
        public Repository(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            return $"Name: {Name} | Description: {Description} | Id: {Id}";
        }
    }
}
