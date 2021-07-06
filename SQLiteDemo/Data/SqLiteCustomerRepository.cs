using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using SQLiteDemo.Model;

namespace SQLiteDemo.Data
{
    public class SqLiteCustomerRepository<T> : SqLiteBaseRepository, ICustomerRepository<T>
    {
        public static SqliteConnection cnn = SimpleDbConnection();

        public static void con ()
        {
            cnn.Open();
        }
        public T Get<T>(long id,string sql)
        {
            if (!File.Exists(DbFile)) return default(T);


                T result = cnn.Query<T>(
                    sql, new { id }).FirstOrDefault();
                return result;
            
        }

        public void Save(T customer,string sql)
        {

          SqliteTransaction sqliteTransaction=   cnn.BeginTransaction();
            try
            {
                cnn.Query<long>(
                    sql, customer).First();
                sqliteTransaction.Commit();
            }
            catch (System.Exception)
            {

                sqliteTransaction.Rollback();
            }
            

        }

        public static void CreateDatabase(string createdb)
        {
            con();
                cnn.Execute(createdb
                    );
            
        }
    }
}
