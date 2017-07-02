using System.Windows.Forms;
using System.Web;

namespace PdfReport_Iran.PDFReport
{
	public static class AppPath
	{
		public static string ApplicationPath
		{
			get
			{
				if (isInWeb)
					return HttpRuntime.AppDomainAppPath;
				//آدرس دهی در برنامه تحت ویندوز
				return Application.StartupPath;
				//آدرس دهی در برنامه تحت وب
				//return Server.MapPath;
			}
		}

		private static bool isInWeb
		{
			get
			{
				return HttpContext.Current != null;
			}
		}
	}
}