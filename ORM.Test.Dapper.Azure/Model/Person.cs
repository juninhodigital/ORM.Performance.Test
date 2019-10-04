using System;
using System.Collections.Generic;
using System.Text;

namespace ORM.Test.Dapper.Azure
{
    public class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool Status { get; set; }

    }
}
