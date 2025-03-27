using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task1.DAL;
using Task1.Models;

namespace Task1.Controllers
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
        //Insertion Controller
        public ActionResult InsertEmployee()
        {
            Employee employee = new Employee()
            {
                FirstName = "Himanshi",
                MiddleName = "J",
                LastName = "Gandhi",
                DOB = new DateTime(2003, 10, 18),
                Address = "Ahmedabad"
            };
            employeeDAL.InsertEmployee(employee);
            return RedirectToAction("Index");
        }
        //Update First Record Controller
        public ActionResult UpdateFirstRecord()
        {
            employeeDAL.UpdateFirstRecord();
            return RedirectToAction("Index");
        }
        //Update Middle Name Controller
        public ActionResult UpdateMiddleRecord()
        {
            employeeDAL.UpdateMiddleName();
            return RedirectToAction("Index");
        }
        //Delete Record Controller
        public ActionResult DeleteRecord()
        {
            employeeDAL.DeleteRecordLessThan2();
            return RedirectToAction("Index");
        }
        //Delete All Record Controller
        public ActionResult DeleteAllRecord()
        {
            employeeDAL.DeleteAllRecords();
            return RedirectToAction("Index");
        }
    }
}