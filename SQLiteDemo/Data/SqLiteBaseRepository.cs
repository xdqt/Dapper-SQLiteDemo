using System;
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

            //if (connection.State == ConnectionState.Closed)
            //{

            //    connection.Open();
            //    var createTableSqlStr = @"CREATE TABLE  if not exists ""Alarm"" ( ""Id"" INTEGER NOT NULL, ""AlarmName"" TEXT, ""AlarmTypeId"" INTEGER, PRIMARY KEY ( ""Id"" ) );";
            //    var result = connection.Execute(createTableSqlStr);//使用Dapper执行sql语句创建表
            //    Console.ReadKey();
            //}






        }
    }
}