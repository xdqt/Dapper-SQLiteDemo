using System.Data.Common;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.Data.Sqlite;
using SQLiteDemo.Model;

namespace SQLiteDemo.Data
{
    public class SqLiteCustomerRepository<T> : SqLiteBaseRepository, ICustomerRepository<T>
    {


        public T Get<T>(long id, string sql)
        {
            if (!File.Exists(DbFile)) return default(T);

            var cnn = SqliteConnectionPool.GetConnection();

            T result = cnn.Query<T>(
               sql, new { id }).FirstOrDefault();



            SqliteConnectionPool.ReleaseConnection(cnn);
            return result;
        }

        public void Save(T customer, string sql)
        {



            var cnn = SqliteConnectionPool.GetConnection();
            {
                DbTransaction sqliteTransaction = cnn.BeginTransaction();
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
            SqliteConnectionPool.ReleaseConnection(cnn);

        }

        public static void CreateDatabase(string createdb)
        {

            var cnn = SqliteConnectionPool.GetConnection();
            {
                cnn.Execute(createdb
                    );
            }
            SqliteConnectionPool.ReleaseConnection(cnn);
        }
    }
}
