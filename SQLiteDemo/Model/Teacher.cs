using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteDemo.Model
{
    public class Teacher
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
