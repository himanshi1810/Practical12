using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Task2.Models;

namespace Task2.DAL
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
                string query = "SELECT * FROM Employee1";
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
                            MobileNumber = reader["MobileNumber"] as string,
                            Address = reader["Address"] as string,
                            Salary = Convert.ToDecimal(reader["Salary"])
                        });
                    }
                }
            }
            return employees;
        }
        //   insert query to insert a record in the above table
        public void InsertEmployee(Employee employee)
        {
            using (SqlConnection conn = new SqlConnection(con))
            {
                string query = "INSERT INTO Employee1 (FirstName, MiddleName, LastName, DOB, MobileNumber, Address, Salary) VALUES (@FirstName, @MiddleName, @LastName, @DOB, @MobileNumber, @Address, @Salary)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", employee.MiddleName);
                cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                cmd.Parameters.AddWithValue("@DOB", employee.DOB);
                cmd.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
                cmd.Parameters.AddWithValue("@Address", employee.Address);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        //SQL query to find the total amount of salaries
        public decimal TotalSalary()
        {
            decimal totalSalary = 0; 

            using (SqlConnection conn = new SqlConnection(con))
            {
                string query = "SELECT SUM(Salary) FROM Employee1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                object result = cmd.ExecuteScalar(); 

                if (result != DBNull.Value)
                {
                    totalSalary = Convert.ToDecimal(result);
                }
            }
            return totalSalary;
        }

        //SQL query to find all employees having DOB less than 01-01-2000
        public List<Employee> DobLessThan()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(con))
            {
                string query = "SELECT * FROM Employee1 WHERE DOB < '2000-01-01'";
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
                            MobileNumber = reader["MobileNumber"].ToString(),
                            Address = reader["Address"] as string,
                            Salary = Convert.ToDecimal(reader["Salary"])
                        });
                    }
                }
            }
            return employees;
        }

        //SQL query to count employees having Middle Name NULL
        public int MiddleNameNull()
        {
            int cnt = 0;
            using (SqlConnection conn = new SqlConnection(con))
            {
                string query = "SELECT COUNT(*) FROM Employee1 WHERE MiddleName IS NULL";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    cnt = Convert.ToInt32(result);
                }
            }
            return cnt;
        }
    }
}