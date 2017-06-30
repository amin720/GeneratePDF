using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PdfReportSamples.IList;
using PdfReportSamples.InMemory;

namespace PdfReport_Iran.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
	    public void Flush2()
	    {
		    // To view its implementation, right click on the method and then select `go to implementation` 
		    new InMemoryPdfReport().CreatePdfReport();
	    }

	    public ActionResult Flush1()
	    {
		    var rpt = new IListPdfReport().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return File(outputFilePath, "application/pdf", "pdfRpt.pdf");
	    }

	    public ActionResult Browse()
	    {
		    var rpt = new IListPdfReport().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
	    }
	}
}