using System.Web;
using System.Web.Mvc;

namespace ORM.Test.Dapper.Azure
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
