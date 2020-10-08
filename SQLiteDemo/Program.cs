using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLiteDemo.Data;
using SQLiteDemo.Model;

namespace SQLiteDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            SqLiteCustomerRepository<Customer>.CreateDatabase(@"CREATE TABLE IF NOT EXISTS Customer
                      (
                         ID                                  integer primary key AUTOINCREMENT,
                         FirstName                           varchar(100) not null,
                         LastName                            varchar(100) not null,
                         DateOfBirth                         datetime not null
                      )");

            SqLiteCustomerRepository<Customer> rep = new SqLiteCustomerRepository<Customer>();
            var customer = new Customer
                {
                    FirstName = "Sergey",
                    LastName = "Maskalik",
                    DateOfBirth = DateTime.Now
                };
            rep.Save(customer, @"INSERT INTO Customer 
                    ( FirstName, LastName, DateOfBirth ) VALUES 
                    ( @FirstName, @LastName, @DateOfBirth );
                    select last_insert_rowid()");

            Customer retrievedCustomer = rep.Get<Customer>(customer.Id, @"SELECT Id, FirstName, LastName, DateOfBirth
                    FROM Customer
                    WHERE Id = @id");



            SqLiteCustomerRepository<Teacher> repp = new SqLiteCustomerRepository<Teacher>();
            var Teacher = new Teacher
            {
                FirstName = "Sergey",
                LastName = "Maskalik",
                DateOfBirth = DateTime.Now
            };
            SqLiteCustomerRepository<Teacher>.CreateDatabase(@"CREATE TABLE IF NOT EXISTS Teacher
                      (
                         ID                                  integer primary key AUTOINCREMENT,
                         FirstName                           varchar(100) not null,
                         LastName                            varchar(100) not null,
                         DateOfBirth                         datetime not null
                      )");
            repp.Save(Teacher, @"INSERT INTO Teacher 
                    ( FirstName, LastName, DateOfBirth ) VALUES 
                    ( @FirstName, @LastName, @DateOfBirth );
                    select last_insert_rowid()");

            Teacher retrievedTeacher = repp.Get<Teacher>(Teacher.Id, @"SELECT Id, FirstName, LastName, DateOfBirth
                    FROM Teacher
                    WHERE Id = @id");



        }
    }
}
