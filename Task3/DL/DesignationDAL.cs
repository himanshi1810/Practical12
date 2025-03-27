using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Task3.DL
{
	public class DesignationDAL
	{
        public int InsertDesignation(string designation)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand("INSERT INTO Designation (Designation) VALUES (@Designation); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@Designation", designation);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
        // a stored procedure to insert data into the Designation table with required parameters
        public int InsertDesignationSP(string designation)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand("InsertDesignation", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Designation", designation);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

}