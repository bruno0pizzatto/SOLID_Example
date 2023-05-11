using SOLID_Example.Interfaces;
using SOLID_Example.Models;
using LiteDB;

namespace SOLID_Example.Databases
{
    public class DBLiteManager<Element> : IDatabase<Element> where Element : BaseEntity
    {
        public LiteDatabase Database { get; private set; }
        public DBLiteManager()
        {
            var database = new LiteDatabase(@"C:\Temp\MyData.db");
            Database = database;
        }
        public ILiteCollection<Element> MapTable() => Database.GetCollection<Element>(typeof(Element).Name);
        public IEnumerable<Element> GetAll()
        {
            var elementsCollection = MapTable();
            return elementsCollection.Query().ToList();
        }

        public bool Add(Element element)
        {
            element.Id = ObjectId.NewObjectId().ToString();
            var elementsCollection = MapTable();

            try
            {
                var insertedElement = elementsCollection.Insert(element);
                return true;
            }
            catch (Exception ex)//TODO: Adding logging
            {
                return false;
            }
        }

        public bool Remove(Element element)
        {
            var elementsCollection = MapTable();
           
            try
            {
                return elementsCollection.Delete(element.Id);
            }
            catch (Exception ex)//TODO: Adding logging
            {
                return false;
            }
        }

        public bool Update(Element element)
        {
            var elementsCollection = MapTable();
            
            try
            {
                return elementsCollection.Update(element);
            }
            catch (Exception ex)//TODO: Adding logging
            {
                return false;
            }
        }

        public void CleanAll()
        {
            var elementsCollection = MapTable();
            elementsCollection.DeleteAll();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<Element> GetElements() => MapTable().Query().ToEnumerable();
    }
}
