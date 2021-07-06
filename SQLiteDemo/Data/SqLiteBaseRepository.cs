using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace SQLiteDemo.Data
{
    public class SqLiteBaseRepository
    {
        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\SimpleDb.sqlite"; }
        }

        public static SqliteConnection SimpleDbConnection()
        {
            
            var connStr = @"Data Source=" + DbFile;//连接字符串

            var conn = new SqliteConnectionStringBuilder(connStr)
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = "password"
            }.ToString();//使用这个方式设置密码，避免sql注入

            return new SqliteConnection(conn);//创建SQLite连接

            
        }
    }


    //使用连接池连接
    public static class SqliteConnectionPool
    {
        private const int PoolSize = 10;

        private static readonly object Locker = new object();

        static SqliteConnectionPool()
        {
            Pool = new List<DbConnection>(PoolSize);
            Additional = new List<DbConnection>();
        }
        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\SimpleDb.sqlite"; }
        }
        private static bool Finalized { get; set; }

        private static int CreatedConnectionsMax => PoolSize;
        private static int CreatedConnectionsCount { get; set; }

        private static List<DbConnection> Pool { get; }
        private static List<DbConnection> Additional { get; }

        public static DbConnection GetConnection()
        {
            if (Finalized)
            {
                throw new Exception("Connection pool was finalized.");
            }

            lock (Locker)
            {
                if (Pool.Count == 0)
                {
                    var connection = CreateAndOpenConnection();
                    if (CreatedConnectionsCount == CreatedConnectionsMax)
                    {
                        Additional.Add(connection);
                        LogAdditionalConnection();
                        return connection;
                    }
                    else
                    {
                        CreatedConnectionsCount++;
                        Pool.Add(connection);
                        return connection;
                    }
                }
                else
                {
                    var connection = Pool[0];
                    Pool.RemoveAt(0);
                    return connection;
                }
            }
        }

        public static void ReleaseConnection(DbConnection connection)
        {
            if (Finalized)
            {
                CloseAndDisposeConnection(connection);
                return;
            }

            lock (Locker)
            {
                if (Additional.Contains(connection))
                {
                    Additional.Remove(connection);
                    CloseAndDisposeConnection(connection);
                }
                else
                {
                    Pool.Add(connection);
                }
            }
        }

        public static void Dispose()
        {
            lock (Locker)
            {
                foreach (var connection in Pool)
                {
                    CloseAndDisposeConnection(connection);
                }

                Finalized = true;
            }
        }

        private static DbConnection CreateAndOpenConnection()
        {
            // create connection string

            var connStr = @"Data Source=" + DbFile;//连接字符串

            var connectionString = new SqliteConnectionStringBuilder(connStr)
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = "password"
            }.ToString();//使用这个方式设置密码，避免sql注入
            // create and open connection
            var conn = new SqliteConnection(connectionString);
            conn.Open();

            // enable write-ahead log
            var cmd = conn.CreateCommand();
            cmd.CommandText = "PRAGMA journal_mode = 'wal'";
            cmd.ExecuteNonQuery();

            return conn;
        }

        private static void CloseAndDisposeConnection(DbConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
            connection.Dispose();
        }

        private static void LogAdditionalConnection()
        {
            // it's important to know if your code is frequently opening additional connections
            // that way you can increase the pool size to improve performance
        }
    }
}