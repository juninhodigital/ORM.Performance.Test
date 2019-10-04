using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Dapper;

namespace ORM.Test.Premisse.Dapper
{
    class Program
    {
        #region| Constructor |

        static void Main(string[] args)
        {
            InsertThousands();
            GetThousands();

            Console.ReadLine();
        }

        #endregion

        #region| Methods |

        /// <summary>
        /// Insert 10.000 rows using Dapper
        /// </summary>
        static void InsertThousands()
        {
            var items = GetMockedData(10000);

            using (var profiler = new Framework.Core.MiniProfiler("Test_Dapper"))
            {
                var statement = "INSERT INTO TB_PERSON(FirstName,LastName,Address,City,ZipCode,Country,Status) VALUES (@FirstName,@LastName,@Address,@City,@ZipCode,@Country,@Status)";

                using (IDbConnection db = new SqlConnection("Password=p@ssw0rd;Persist Security Info=True;User ID=sa;Initial Catalog=DB_Test;Data Source=LOCALHOST"))
                {
                    db.Open();

                    foreach (var item in items)
                    {
                        db.Execute(statement,
                        new
                        {
                            item.FirstName,
                            item.LastName,
                            item.Address,
                            item.City,
                            item.ZipCode,
                            item.Country,
                            item.Status
                        });

                    }
                }

                var total = profiler.GetElapsedTime();

                Console.WriteLine($"Elapsed time using Dapper (INSERT): {total}");
            }

        }

        /// <summary>
        /// Get 10.000 rows using Dapper
        /// </summary>
        static void GetThousands()
        {
            using (var profiler = new Framework.Core.MiniProfiler("Test_Dapper"))
            {
                var statement = "SELECT * FROM TB_PERSON";

                using (IDbConnection db = new SqlConnection("Password=p@ssw0rd;Persist Security Info=True;User ID=sa;Initial Catalog=DB_Test;Data Source=LOCALHOST"))
                {
                    var items = db.Query<Person>(statement).ToList();
                }

                var total = profiler.GetElapsedTime();

                Console.WriteLine($"Elapsed time using Dapper (SELECT): {total}");
            }

        }

        /// <summary>
        /// Get mocked data to insert rows
        /// </summary>
        static List<Person> GetMockedData(int total)
        {
            var output = new List<Person>();

            for (int i = 0; i < total; i++)
            {
                var randon = new Random().Next(156, 9874465);
                var item = new Person
                {
                    Address   = $"Adress - {randon}",
                    City      = $"City - {randon}",
                    Country   = $"Country - {randon}",
                    FirstName = $"FirstName - {randon}",
                    LastName  = $"LastName - {randon}",
                    ZipCode   = $"ZipCode - {randon}".Substring(0, 10),
                    Status    = new Random().Next(0, 1) == 0 ? false : true
                };

                output.Add(item);
            }

            return output;
        } 

        #endregion
    }
}
