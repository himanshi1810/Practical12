using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Task_3.Models;

namespace Task_3.DAL
{
	public class EmployeeDAL
	{
        private string connString;
        public EmployeeDAL()
        {
            connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        //Get Employee Details
        public List<Employee> getEmployees()
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT FirstName, MiddleName, LastName, Address, DOB, MobileNumber,Salary,DesignationId FROM Employee2";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Address = reader["Address"].ToString(),
                                DOB = reader.GetDateTime(reader.GetOrdinal("DOB")),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                Salary = reader.GetDecimal(reader.GetOrdinal("Salary")), 
                                DesignationId = reader.GetInt32(reader.GetOrdinal("DesignationId")) 
                            });
                        }
                    }
                }
            }
            return employees;
        }
        //Get Designation
        public List<Designation> getDesignations()
        {
            List<Designation> designations = new List<Designation>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT Id,Designation FROM Designation";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            designations.Add(new Designation() { Id = (int)reader["Id"], DesignationName = reader["Designation"].ToString() });
                        }
                    }
                }
            }
            return designations;
        }
        //Insert Employee
        public void InsertEmployee(Employee e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_InsertEmployee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Value = e.FirstName });
                    cmd.Parameters.Add(new SqlParameter("@MiddleName", SqlDbType.NVarChar, 50) { Value = e.MiddleName });
                    cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Value = e.LastName });
                    cmd.Parameters.Add(new SqlParameter("@DesignationId", SqlDbType.Int) { Value = e.DesignationId });
                    cmd.Parameters.Add(new SqlParameter("@DOB", SqlDbType.Date) { Value = e.DOB });
                    cmd.Parameters.Add(new SqlParameter("@MobileNumber", SqlDbType.NVarChar, 10) { Value = e.MobileNumber });
                    cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar) { Value = e.Address });
                    cmd.Parameters.Add(new SqlParameter("@Salary", SqlDbType.Decimal) { Value = e.Salary });

                    cmd.ExecuteNonQuery();
                }
            }
        }
        //Insert Designation
        public void InsertDesignation(Designation d)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_InsertDesignation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Designation", SqlDbType.NVarChar, 50) { Value = d.DesignationName });
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //Create a database view that outputs Employee Id, First Name, Middle Name, Last Name, Designation, DOB, Mobile Number, Address & Salary
        public List<EmployeeDesignation> getEmployeeWithDesignation()
        {
            List<EmployeeDesignation> employees = new List<EmployeeDesignation>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT e.FirstName, e.MiddleName, e.LastName, d.Designation FROM Employee2 e JOIN Designation d ON e.DesignationId = d.Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new EmployeeDesignation
                            {
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                DesignationName = reader["Designation"].ToString()
                            });
                        }
                    }
                }
            }
            return employees;
        }
        // group by desgination in employee table and get the count of employees in each designation
        public List<DesignationCount> getDesignationCount()
        {
            List<DesignationCount> employees = new List<DesignationCount>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT d.Designation, COUNT(e.Id) AS EmployeeCount FROM Employee2 e JOIN Designation d ON e.DesignationId = d.Id GROUP BY d.Designation";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new DesignationCount
                            {
                                DesignationName = reader["Designation"].ToString(),
                                Count = (int)reader["EmployeeCount"]
                            });
                        }
                    }
                }
                return employees;
            }
        }
        //Get Designation With More than One Employee
        public List<DesignationCount> getDesignationWithMoreThanOneEmployee()
        {
            List<DesignationCount> designations = new List<DesignationCount>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT d.Designation, COUNT(e.Id) AS EmployeeCount FROM Employee2 e JOIN Designation d ON e.DesignationId = d.Id GROUP BY d.Designation HAVING COUNT(e.Id) > 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            designations.Add(new DesignationCount
                            {
                                DesignationName = reader["Designation"].ToString(),
                                Count = (int)reader["EmployeeCount"]
                            });
                        }
                    }
                }
            }
            return designations;
        }
        //To find the employee having maximum salary
        public Employee getEmployeeWithMaxSalary()
        {
            Employee employee = null;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT TOP 1 * FROM Employee2 ORDER BY Salary DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            employee = new Employee
                            {
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Address = reader["Address"].ToString(),
                                DOB = reader.GetDateTime(reader.GetOrdinal("DOB")), // Reads as DateTime
                                MobileNumber = reader["MobileNumber"].ToString(),
                                Salary = reader.GetDecimal(reader.GetOrdinal("Salary")), // Reads as Decimal
                                DesignationId = reader.GetInt32(reader.GetOrdinal("DesignationId")) // Reads as Int
                            };
                        }
                    }
                }
            }
            return employee;
        }
        //Employee Details View
        public List<EmployeeDetailsView> EmployeeDetailsViews()
        {
            List<EmployeeDetailsView> employees = new List<EmployeeDetailsView>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT * FROM vw_EmployeeDetails    ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new EmployeeDetailsView
                            {
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader.IsDBNull(reader.GetOrdinal("MiddleName")) ? null : reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                DesignationName = reader["Designation"].ToString(),
                                DOB = reader.GetDateTime(reader.GetOrdinal("DOB")),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                Address = reader["Address"].ToString(),
                                Salary = reader.GetDecimal(reader.GetOrdinal("Salary")) 
                            });
                        }
                    }
                }
            }
            return employees;
        }
        //Stored Procedure 
        public List<EmployeeDetailsView> GetAllEmployeesStoredProcedure()
        {
            List<EmployeeDetailsView> employees = new List<EmployeeDetailsView>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetAllEmployees", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new EmployeeDetailsView
                            {
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                DesignationName = reader["Designation"].ToString(),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                Address = reader["Address"].ToString(),
                                DOB = Convert.ToDateTime(reader["DOB"]),
                                Salary = Convert.ToDecimal(reader["Salary"])
                            });
                        }
                    }
                }
            }
            return employees;
        }
        public List<EmployeeDetailsView> GetEmployeesByDesignation(int designationId)
        {
            List<EmployeeDetailsView> employees = new List<EmployeeDetailsView>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetEmployeesByDesignation", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@DesignationId", SqlDbType.Int) { Value = designationId });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new EmployeeDetailsView
                            {
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                Address = reader["Address"].ToString(),
                                DOB = Convert.ToDateTime(reader["DOB"]),
                                Salary = Convert.ToDecimal(reader["Salary"])
                            });
                        }
                    }
                }
            }
            return employees;
        }

    }
}