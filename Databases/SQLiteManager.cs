using SOLID_Example.Interfaces;
using SOLID_Example.Models;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Xml.Linq;


namespace SOLID_Example.Databases
{
    public class SQLiteManager<Element> : IDatabase<Element> where Element : BaseEntity
    {
        private const string DATABASE_PATH = @"C:/Dados/banco_temp.sqlite";
        private static SQLiteConnection _connection;

        public SQLiteManager()
        {
            createSQLiteTable();

            var doesntExistPath = !File.Exists(@"C:/Dados");
            if (doesntExistPath)
            {
                File.Create(@"C:/Dados");
            }

            var doesntExistsDatabase = !File.Exists(DATABASE_PATH);
            if (doesntExistsDatabase)
            {
                SQLiteConnection.CreateFile(DATABASE_PATH);
            }
        }
        private string getTableName() => typeof(Element).Name;

        public static void createSQLiteTable()
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Funcionario(id int, Nome Varchar(300), Salario Real)";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Documento(id int, Nome Varchar(300), Descricao VarChar(300))";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static SQLiteConnection DbConnection()
        {
            _connection = new SQLiteConnection("Data Source=c:\\dados\\Cadastro.sqlite; Version=3;");
            _connection.Open();
            return _connection;
        }

        public IEnumerable<Element> GetAll()
        {
            SQLiteDataAdapter dataAdaptador = null;
            DataTable elementsCollection = new DataTable();

            try
            {
                using (var comando = DbConnection().CreateCommand())
                {
                    comando.CommandText = $"SELECT * FROM {PegaNomeTabela()}";
                    dataAdaptador = new SQLiteDataAdapter(comando.CommandText, DbConnection());
                    dataAdaptador.Fill(elementsCollection);

                    return ConvertDataTable<Element>(elementsCollection);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string PegaNomeTabela() => typeof(Element).Name;

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
