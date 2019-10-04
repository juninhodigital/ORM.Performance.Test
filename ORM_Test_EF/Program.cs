using System;
using System.Collections.Generic;
using System.Linq;

namespace ORM.Test.Premisse.EF
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
        /// Insert 10.000 rows using EF (DbContext/DbSet)
        /// </summary>
        static void InsertThousands()
        {
            var items = GetMockedData(10000);

            using (var profiler = new Framework.Core.MiniProfiler("Test_EF"))
            {
                using (var context = new DatabaseContext("Password=p@ssw0rd;Persist Security Info=True;User ID=sa;Initial Catalog=DB_Test;Data Source=LOCALHOST"))
                {
                    foreach (var item in items)
                    {
                        context.Persons.Add(item);
                    }

                    context.SaveChanges();
                }

                var total = profiler.GetElapsedTime();

                Console.WriteLine($"Elapsed time using Entity (INSERT): {total}");
            }

        }

        /// <summary>
        /// Get 10.000 rows using Entity Framework
        /// </summary>
        static void GetThousands()
        {
            using (var profiler = new Framework.Core.MiniProfiler("Test_EF"))
            {
                using (var context = new RepoEntities())
                {
                    var items = context.TB_Person.ToList();
                }

                var total = profiler.GetElapsedTime();

                Console.WriteLine($"Elapsed time using Entity (SELECT): {total}");
            }
        }
        
        /// <summary>
        /// Insert 10.000 rows using EF(EDMX)
        /// </summary>
        static void InsertThousands2()
        {
            var items = GetMockedData(10000);

            using (var profiler = new Framework.Core.MiniProfiler("Test_EF"))
            {
                using (var context = new RepoEntities())
                {
                    foreach (var item in items)
                    {
                        context.TB_Person.Add(item);
                    }

                    context.SaveChanges();
                }

                var total = profiler.GetElapsedTime();

                Console.WriteLine($"Elapsed time using Entity: {total}");
            }

        }

        /// <summary>
        /// Get mocked data to insert rows
        /// </summary>
        static List<TB_Person> GetMockedData(int total)
        {
            var output = new List<TB_Person>();

            for (int i = 0; i < total; i++)
            {
                var randon = new Random().Next(156, 9874465);
                var item = new TB_Person
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
