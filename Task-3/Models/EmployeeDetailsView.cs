using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_3.Models
{
	public class EmployeeDetailsView
	{
		 public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DesignationName { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public decimal Salary { get; set; }
	}
}