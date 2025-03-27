using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Task1.Models;

namespace Task1.DAL
{
    public class EmployeeDAL
	{
		private string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        //Select query to get all the records from the table
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(con))
            {
                string query = "SELECT * FROM Employee";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"] as string,
                            LastName = reader["LastName"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            Address = reader["Address"] as string
                        });
                    }
                }
            }
            return employees;
        }
        //   insert query to insert a record in the above table
        public void InsertEmployee(Employee employee)
		{
			using(SqlConnection conn = new SqlConnection(con))
			{
				string query = "INSERT INTO Employee (FirstName, MiddleName, LastName, DOB, Address) VALUES (@FirstName, @MiddleName, @LastName, @DOB, @Address)";
                SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@DOB", employee.DOB);
                cmd.Parameters.AddWithValue("@Address", employee.Address);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
		}
        //Update query to change the First Name to “SQLPerson” for the first record
        public void UpdateFirstRecord()
		{
			using(SqlConnection conn = new SqlConnection(con))
			{
				string query = "UPDATE Employee SET FirstName = 'FirstName' WHERE Id = (SELECT MIN(Id) FROM Employee)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
		}
        //Update query to change the Middle Name to “I” for all records
        public void UpdateMiddleName()
		{
			using(SqlConnection conn = new SqlConnection(con))
			{
				string query = "UPDATE Employee SET MiddleName = 'I'";
				SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
		}
        //Delete query to delete record having Id column value less than 2
        public void DeleteRecordLessThan2()
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                string query = "DELETE FROM Employee WHERE Id < 2";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        //Delete all the data from the table
        public void DeleteAllRecords()
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                string query = "DELETE FROM Employee";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}