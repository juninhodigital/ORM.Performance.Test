using ORM.Test.Dapper.Azure;
using System.Data.Entity;

namespace ORM_Test_EF
{
    class DatabaseContext : DbContext
    {
        public DatabaseContext(string connection): base(connection)
        {

        }
        public DbSet<TB_Person> Persons { get; set; }
    }
}
