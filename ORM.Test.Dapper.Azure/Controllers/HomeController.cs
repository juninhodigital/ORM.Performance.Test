using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

using Dapper;
using ORM_Test_EF;

namespace ORM.Test.Dapper.Azure.Controllers
{
    [RoutePrefix("home")]
    public class HomeController : Controller
    {
        #region| Actions |

        public ActionResult Index()
        {
            InsertThousands();
            GetThousands();

            #region| Reset data |

            using (IDbConnection db = new SqlConnection(Constants.Connection))
            {
                db.Open();

                var rows = db.Execute("DELETE FROM TB_PERSON");
            } 

            #endregion

            InsertThousandsEF();
            GetThousandsEF();

            return View();
        }
        #endregion

        #region| Methods |

        /// <summary>
        /// Insert 10.000 rows using Dapper
        /// </summary>
        private void InsertThousands()
        {
            var items = GetMockedData(10000);

            using (var profiler = new Framework.Core.MiniProfiler("Test_Dapper"))
            {
                var statement = "INSERT INTO TB_PERSON(FirstName,LastName,Address,City,ZipCode,Country,Status) VALUES (@FirstName,@LastName,@Address,@City,@ZipCode,@Country,@Status)";

                using (IDbConnection db = new SqlConnection(Constants.Connection))
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

                ViewBag.ElapsedTimeInsertDapper = $"Elapsed time using Dapper (INSERT): {total}";
            }
        }

        /// <summary>
        /// Get 10.000 rows using Dapper
        /// </summary>
        private void GetThousands()
        {
            using (var profiler = new Framework.Core.MiniProfiler("Test_Dapper"))
            {
                var statement = "SELECT * FROM TB_PERSON";

                using (IDbConnection db = new SqlConnection(Constants.Connection))
                {
                    var items = db.Query<Person>(statement).ToList();
                }

                var total = profiler.GetElapsedTime();

                ViewBag.ElapsedTimeSelectDapper = $"Elapsed time using Dapper (SELECT): {total}";
            }

        }

        /// <summary>
        /// Insert 10.000 rows using EF (DbContext/DbSet)
        /// </summary>
        private void InsertThousandsEF()
        {
            var items = GetMockedDataEF(10000);

            using (var profiler = new Framework.Core.MiniProfiler("Test_EF"))
            {
                using (var context = new DatabaseContext(Constants.Connection))
                {
                    foreach (var item in items)
                    {
                        context.Persons.Add(item);
                    }

                    context.SaveChanges();
                }

                var total = profiler.GetElapsedTime();

                ViewBag.ElapsedTimeInsertEF = $"Elapsed time using Entity (INSERT): {total}";
            }

        }

        /// <summary>
        /// Get 10.000 rows using Entity Framework
        /// </summary>
        private void GetThousandsEF()
        {
            using (var profiler = new Framework.Core.MiniProfiler("Test_EF"))
            {
                using (var context = new RepoEntities())
                {
                    var items = context.TB_Person.ToList();
                }

                var total = profiler.GetElapsedTime();

                ViewBag.ElapsedTimeSelectEF = $"Elapsed time using Entity (SELECT): {total}";
            }
        }

        /// <summary>
        /// Get mocked data to insert rows
        /// </summary>
        private List<Person> GetMockedData(int total)
        {
            var output = new List<Person>();

            for (int i = 0; i < total; i++)
            {
                var randon = new Random().Next(156, 9874465);
                var item = new Person
                {
                    Address = $"Adress - {randon}",
                    City = $"City - {randon}",
                    Country = $"Country - {randon}",
                    FirstName = $"FirstName - {randon}",
                    LastName = $"LastName - {randon}",
                    ZipCode = $"ZipCode - {randon}".Substring(0, 10),
                    Status = new Random().Next(0, 1) == 0 ? false : true
                };

                output.Add(item);
            }

            return output;
        }

        /// <summary>
        /// Get mocked data to insert rows
        /// </summary>
        private List<TB_Person> GetMockedDataEF(int total)
        {
            var output = new List<TB_Person>();

            for (int i = 0; i < total; i++)
            {
                var randon = new Random().Next(156, 9874465);
                var item = new TB_Person
                {
                    Address = $"Adress - {randon}",
                    City = $"City - {randon}",
                    Country = $"Country - {randon}",
                    FirstName = $"FirstName - {randon}",
                    LastName = $"LastName - {randon}",
                    ZipCode = $"ZipCode - {randon}".Substring(0, 10),
                    Status = new Random().Next(0, 1) == 0 ? false : true
                };

                output.Add(item);
            }

            return output;
        } 

        #endregion
    }
}