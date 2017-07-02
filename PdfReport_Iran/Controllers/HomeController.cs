using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PdfReportSamples.IList;
using PdfReportSamples.InMemory;
using PdfReport_Iran.PDFReport;

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

	    public ActionResult Amin()
	    {
			var rpt = new IList().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
		}

	    public ActionResult CustomHeaderFooterPdfReport()
	    {
		    var rpt = new CustomHeaderFooterPdfReport().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
	    }
public ActionResult CustomHeader()
	    {
		    var rpt = new CustomHeaderFooterPdfReport().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
	    }

	    public ActionResult HtmlHeaderRtl()
	    {
		    var rpt = new HtmlHeaderRtl().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
	    }

	    public ActionResult CustomHtmlHeaderRtl()
	    {
		    var rpt = new CustomHeaderDoc().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
	    }

	    public ActionResult InlineProviders()
	    {
		    var rpt = new InlineProvidersPdfReport().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
	    }

	    public ActionResult InlineCustomDoc()
	    {
		    var rpt = new InlineCustonDoc().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
	    }
	    public ActionResult KhodaDorstBeshe()
	    {
		    var rpt = new KhodaDorstBeshe().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
	    }
public ActionResult AcroFormTemplate()
	    {
		    var rpt = new AcroFormTemplate().CreatePdfReport();
		    var outputFilePath = rpt.FileName.Replace(HttpRuntime.AppDomainAppPath, string.Empty);
		    return Redirect(outputFilePath);
	    }

	}
}