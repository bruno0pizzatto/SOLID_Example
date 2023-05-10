using SOLID_Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
