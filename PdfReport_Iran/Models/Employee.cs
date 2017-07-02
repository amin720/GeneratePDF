using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PdfReport_Iran.Models
{
	public class Employee
	{
		public int Id { set; get; }
		public string Name { set; get; }
		public string Department { set; get; }
		public decimal Salary { set; get; }
		public int Age { set; get; }
		public string WorkedHours { set; get; }
	}
}