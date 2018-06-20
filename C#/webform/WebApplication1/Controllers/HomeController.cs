using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var sqlConStrBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder()
            {
                DataSource = "."

            };
            sqlConStrBuilder.InitialCatalog = "testDB";
            sqlConStrBuilder.IntegratedSecurity = true;
            string sqlConStr = sqlConStrBuilder.ConnectionString;

            using (SqlConnection con = new SqlConnection(sqlConStr))
            {
                using (SqlCommand com = new SqlCommand("select * from [Order]", con))
                {
                    com.Parameters.Add(new SqlParameter());

                    con.Open();
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        Debug.WriteLine(reader["Name"]);
                    }

                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}