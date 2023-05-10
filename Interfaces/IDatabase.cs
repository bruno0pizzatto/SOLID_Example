using SOLID_Example.Models;

namespace SOLID_Example.Interfaces
{
    public interface IDatabase<Element> where Element : BaseEntity
    {
        IEnumerable<Element> GetAll();
        bool Add(Element element);
        bool Remove(Element element);
        bool Update(Element element);
        void CleanAll();
        void Dispose();
        IEnumerable<Element> GetElements();
    }
}
