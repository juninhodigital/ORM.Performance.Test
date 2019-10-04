using System.Data.Entity;

namespace ORM.Test.Premisse.EF
{
    class DatabaseContext : DbContext
    {
        #region| Properties | 

        public DbSet<TB_Person> Persons { get; set; } 

        #endregion

        #region| Constructor |

        public DatabaseContext(string connection) : base(connection)
        {

        } 

        #endregion
    }
}
