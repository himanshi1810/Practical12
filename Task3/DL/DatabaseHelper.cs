using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Task3.DL
{
	public class DatabaseHelper
	{
        private static string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(con);
        }
    }
}