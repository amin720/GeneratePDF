using System.Web;
using System.Windows.Forms;

namespace PdfReport
{
	/// <summary>
	///  این کلاس برای مشخص سازی محل ذخیره سازی فایل‌های نهایی
	/// PDF تولیدی استفاده خواهیم کرد
	/// </summary>
	public static class AppPath
	{
		public static string ApplicationPath
		{
			get
			{
				if (isInWeb)
					return HttpRuntime.AppDomainAppPath;

				return Application.StartupPath;
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
