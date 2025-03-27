using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Task3.Models;

namespace Task3.DL
{
	public class EmployeeDAL
	{
        //Insert Employee
        public int InsertEmployee(Employee employee)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand(@"
            INSERT INTO Employee2
            (FirstName, MiddleName, LastName, DOB, MobileNumber, Address, Salary, DesignationId) 
            VALUES 
            (@FirstName, @MiddleName, @LastName, @DOB, @MobileNumber, @Address, @Salary, @DesignationId);
            SELECT SCOPE_IDENTITY();", connection);

                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@MiddleName", (object)employee.MiddleName ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@DOB", employee.DOB);
                command.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
                command.Parameters.AddWithValue("@Address", (object)employee.Address ?? DBNull.Value);
                command.Parameters.AddWithValue("@Salary", employee.Salary);
                command.Parameters.AddWithValue("@DesignationId", (object)employee.DesignationId ?? DBNull.Value);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
        //count the number of records by designation name
        public Dictionary<string, int> GetEmployeeCountByDesignation()
        {
            var result = new Dictionary<string, int>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand(@"
            SELECT d.Designation, COUNT(e.Id) AS EmployeeCount
            FROM Designation d
            LEFT JOIN Employee2 e ON d.Id = e.DesignationId
            GROUP BY d.Designation", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader["Designation"].ToString(), Convert.ToInt32(reader["EmployeeCount"]));
                    }
                }
            }

            return result;
        }
        //to display First Name, Middle Name, Last Name & Designation name
        public List<EmployeeDesignationViewModel> GetEmployeesWithDesignation()
        {
            var employees = new List<EmployeeDesignationViewModel>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand(@"
            SELECT e.FirstName, e.MiddleName, e.LastName, d.Designation
            FROM Employee2 e
            LEFT JOIN Designation d ON e.DesignationId = d.Id", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new EmployeeDesignationViewModel
                        {
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"] is DBNull ? null : reader["MiddleName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Designation = reader["Designation"] is DBNull ? null : reader["Designation"].ToString()
                        });
                    }
                }
            }

            return employees;
        }
        //database view that outputs Employee Id, First Name, Middle Name, Last Name, Designation, DOB, Mobile Number, Address & Salary
        public List<EmployeeDetailsViewModel> GetEmployeeDetailsFromView()
        {
            var employees = new List<EmployeeDetailsViewModel>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand("SELECT * FROM vw_EmployeeDetails", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new EmployeeDetailsViewModel
                        {
                            EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"] is DBNull ? null : reader["MiddleName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Designation = reader["Designation"] is DBNull ? null : reader["Designation"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            MobileNumber = reader["MobileNumber"].ToString(),
                            Address = reader["Address"] is DBNull ? null : reader["Address"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"])
                        });
                    }
                }
            }

            return employees;
        }
        //stored procedure to insert data into the Employee table with required parameters
        public int InsertEmployeeWithSP(Employee employee)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand("sp_InsertEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@MiddleName", (object)employee.MiddleName ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@DOB", employee.DOB);
                command.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
                command.Parameters.AddWithValue("@Address", (object)employee.Address ?? DBNull.Value);
                command.Parameters.AddWithValue("@Salary", employee.Salary);
                command.Parameters.AddWithValue("@DesignationId", (object)employee.DesignationId ?? DBNull.Value);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
        //a query that displays only those designation names that have more than 1 employee
        public List<string> GetDesignationsWithMultipleEmployees()
        {
            var designations = new List<string>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand(@"
            SELECT d.Designation
            FROM Designation d
            JOIN Employee2 e ON d.Id = e.DesignationId
            GROUP BY d.Designation
            HAVING COUNT(e.Id) > 1", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        designations.Add(reader["Designation"].ToString());
                    }
                }
            }

            return designations;
        }
        //a stored procedure that returns a list of employees with columns Employee Id, First Name, Middle Name, Last Name, Designation, DOB, Mobile Number, Address & Salary (records should be ordered by DOB)
        public List<EmployeeDetailsViewModel> GetEmployeesOrderedByDOB()
        {
            var employees = new List<EmployeeDetailsViewModel>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand("sp_GetEmployeesOrderByDOB", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new EmployeeDetailsViewModel
                        {
                            EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"] is DBNull ? null : reader["MiddleName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Designation = reader["Designation"] is DBNull ? null : reader["Designation"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            MobileNumber = reader["MobileNumber"].ToString(),
                            Address = reader["Address"] is DBNull ? null : reader["Address"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"])
                        });
                    }
                }
            }

            return employees;
        }
        // stored procedure that return a list of employees by designation id (Input) with columns Employee Id, First Name, Middle Name, Last Name, DOB, Mobile Number, Address & Salary (records should be ordered by First Name)
        public List<Employee> GetEmployeesByDesignationId(int designationId)
        {
            var employees = new List<Employee>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand("sp_GetEmployeesByDesignationId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DesignationId", designationId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = Convert.ToInt32(reader["EmployeeId"]),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"] is DBNull ? null : reader["MiddleName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            MobileNumber = reader["MobileNumber"].ToString(),
                            Address = reader["Address"] is DBNull ? null : reader["Address"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            DesignationId = designationId
                        });
                    }
                }
            }

            return employees;
        }
        //Non-Clustered index on the DesignationId column of the Employee table
        public void CreateDesignationIdIndex()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand("CREATE NONCLUSTERED INDEX IX_Employee_DesignationId ON Employee(DesignationId);", connection);
                command.ExecuteNonQuery();
            }
        }
        //a query to find the employee having maximum salary
        public Employee GetEmployeeWithMaxSalary()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = new SqlCommand(@"
            SELECT TOP 1 
                e.Id, e.FirstName, e.MiddleName, e.LastName, e.DOB, 
                e.MobileNumber, e.Address, e.Salary, e.DesignationId
            FROM Employee2 e
            ORDER BY e.Salary DESC", connection);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Employee
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            MiddleName = reader["MiddleName"] is DBNull ? null : reader["MiddleName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            MobileNumber = reader["MobileNumber"].ToString(),
                            Address = reader["Address"] is DBNull ? null : reader["Address"].ToString(),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            DesignationId = reader["DesignationId"] is DBNull ? (int?)null : Convert.ToInt32(reader["DesignationId"])
                        };
                    }
                }
            }

            return null;
        }
    }
}