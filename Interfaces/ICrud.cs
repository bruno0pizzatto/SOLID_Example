using SOLID_Example.Models;

namespace SOLID_Example.Interfaces
{
    public interface ICrud<Element> where Element : BaseEntity
    {
        public void Add(Element element);
        public void Delete(Element element);
        public void Update(Element element);
        public List<Element> GetAll();  
    }
}
