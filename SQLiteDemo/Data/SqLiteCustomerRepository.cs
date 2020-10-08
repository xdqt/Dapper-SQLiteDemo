using System.IO;
using System.Linq;
using Dapper;
using SQLiteDemo.Model;

namespace SQLiteDemo.Data
{
    public class SqLiteCustomerRepository<T> : SqLiteBaseRepository, ICustomerRepository<T>
    {
        public T Get<T>(long id,string sql)
        {
            if (!File.Exists(DbFile)) return default(T);

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                T result = cnn.Query<T>(
                    sql, new { id }).FirstOrDefault();
                return result;
            }
        }

        public void Save(T customer,string sql)
        {
            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                cnn.Query<long>(
                    sql, customer).First();
            }
        }

        public static void CreateDatabase(string createdb)
        {
            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                cnn.Execute(createdb
                    );
            }
        }
    }
}
