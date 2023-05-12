using SOLID_Example.Interfaces;
using SOLID_Example.Models;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Text;
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
            try//TODO I need to come back here and adjust the tables
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS User(id int, Name Varchar(300), Email Varchar(100))";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Repository(id int, Name Varchar(300), Description VarChar(300))";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)//TODO: Adding logging
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
                    comando.CommandText = $"SELECT * FROM {GetTableName()}";
                    dataAdaptador = new SQLiteDataAdapter(comando.CommandText, DbConnection());
                    dataAdaptador.Fill(elementsCollection);

                    return ConvertDataTable<Element>(elementsCollection);
                }
            }
            catch (Exception ex)//TODO: Adding logging
            {
                throw ex;
            }
        }

        public bool Add(Element element)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    StringBuilder propriertiesWithoutSign = new StringBuilder();
                    propriertiesWithoutSign.Append("(");

                    var proprierties = typeof(Element).GetProperties().ToList();
                    foreach (var property in proprierties)
                    {
                        var fieldValue = element.GetType().GetProperty(property.Name).GetValue(element);

                        if (fieldValue == null)
                            continue;

                        if (proprierties.Last() == property)
                        {
                            propriertiesWithoutSign.Append($" {fieldValue} ");
                        }
                        else
                        {
                            propriertiesWithoutSign.Append($" {fieldValue}, ");
                        }
                    }
                    propriertiesWithoutSign.Append(")");

                    StringBuilder propriertiesWithSign = new StringBuilder();
                    propriertiesWithSign.Append("(");

                    foreach (var property in proprierties)
                    {
                        var fieldValue = element.GetType().GetProperty(property.Name).GetValue(element);

                        if (fieldValue == null)
                            continue;

                        if (proprierties.Last() == property)
                        {
                            propriertiesWithSign.Append($" @{fieldValue} ");
                        }
                        else
                        {
                            propriertiesWithSign.Append($" @{fieldValue}, ");
                        }
                    }
                    propriertiesWithSign.Append(")");

                    cmd.CommandText = $"INSERT INTO {getTableName()} {propriertiesWithoutSign.ToString()} values ({propriertiesWithSign.ToString()})";
                    var commandToExecute = $"UPDATE {getTableName()} SET {propriertiesWithoutSign.ToString()}";

                    foreach (var property in typeof(Element).GetProperties())
                    {
                        var fieldValue = element.GetType().GetProperty(property.Name).GetValue(element);

                        if (fieldValue == null)
                            continue;

                        cmd.Parameters.AddWithValue($"@{fieldValue}", fieldValue);
                    }

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)//TODO: Adding logging
            {
                return false;
            }
        }

        public bool Delete(Element element)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM {getTableName()} WHERE Id = @Id)";
                    cmd.Parameters.AddWithValue("@Id", element.Id);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)//TODO: Adding logging
            {
                return false;
            }
        }

        public void CleanAll()
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    cmd.CommandText = $"DELETE FROM {getTableName()}";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)//TODO: Adding logging
            {
                throw ex;
            }
        }

        public bool Update(Element element)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var property in typeof(Element).GetProperties())
                    {
                        var fieldValue = element.GetType().GetProperty(property.Name).GetValue(element);

                        if (fieldValue == null)
                            continue;

                        stringBuilder.Append($" {fieldValue} = @{fieldValue} ");
                    }

                    var commandToExecute = $"UPDATE {getTableName()} SET {stringBuilder.ToString()}";

                    foreach (var property in typeof(Element).GetProperties())
                    {
                        var fieldValue = element.GetType().GetProperty(property.Name).GetValue(element);

                        if (fieldValue == null)
                            continue;

                        cmd.Parameters.AddWithValue($"@{fieldValue}", fieldValue);
                    }

                    cmd.ExecuteNonQuery();

                };

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Element> GetElements()
        {
            SQLiteDataAdapter dataAdaptador = null;
            DataTable returnedData = new DataTable();
            try
            {
                using (var command = DbConnection().CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {GetTableName()}";
                    dataAdaptador = new SQLiteDataAdapter(command.CommandText, DbConnection());
                    dataAdaptador.Fill(returnedData);

                    return ConvertDataTable<Element>(returnedData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        private string GetTableName() => typeof(Element).Name;

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
