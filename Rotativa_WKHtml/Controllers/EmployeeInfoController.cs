using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using Rotativa_WKHtml.Models;

namespace Rotativa_WKHtml.Controllers
{
	public class EmployeeInfoController : Controller
	{
		AppEntities ctx;

		public EmployeeInfoController()
		{
			ctx = new Models.AppEntities();
		}

		public ActionResult Index()
		{
			var emps = ctx.EmployeeInfoes.ToList();
			return View(emps);
		}

		public ActionResult PrintAllReport()
		{
			var report = new ActionAsPdf("Index");
			return report;
		}

		public ActionResult IndexById(int id)
		{
			var emp = ctx.EmployeeInfoes.First(e => e.EmpNo == id);
			return View(emp);
		}

		public ActionResult PrintSalarySlip(int id)
		{
			var report = new ActionAsPdf("IndexById", new {id = id});
			return report;
		}
	}
}