using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Task2.DAL;
using Task2.Models;

namespace Task2.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeDAL employeeDAL = new EmployeeDAL();

        // GET: Employee
        public ActionResult Index()
        {
            List<Employee> employees = employeeDAL.GetAllEmployees();
            return View(employees);
        }

        // Insertion Controller
        public ActionResult InsertEmployee()
        {
            Employee employee = new Employee()
            {
                FirstName = "Himanshi",
                MiddleName = "J",
                LastName = "Gandhi",
                DOB = new DateTime(2003, 10, 18),
                MobileNumber = "1234567890",
                Address = "Ahmedabad",
                Salary = 10000
            };
            employeeDAL.InsertEmployee(employee);
            return RedirectToAction("Index");
        }

        // Total Salary Controller
        public ActionResult TotalSalary()
        {
            var totalSalary = employeeDAL.TotalSalary();
            return Content("Total Salary: " + totalSalary);
        }

        //DOB Controller
        public ActionResult DobLessThan()
        {
            List<Employee> employees = employeeDAL.DobLessThan();
            return View(employees);
        }
        //Middle Name Controller
        public ActionResult MiddleName()
        {
            var middleNameCount = employeeDAL.MiddleNameNull();
            return Content("Middle Name: " + middleNameCount);
        }
    }
}
