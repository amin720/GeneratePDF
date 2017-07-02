using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deleted
{
	class Program
	{
		static void Main(string[] args)
		{
			//File.Open(@"C:\Users\Eagle720\Documents\Visual Studio 2017\Projects\GeneratePDF\PdfReport_Iran\App_Data\doc_first.pdf",FileMode.Open);
			//File.OpenRead(
			//	@"C:\Users\Eagle720\Documents\Visual Studio 2017\Projects\GeneratePDF\PdfReport_Iran\App_Data\test.txt");
			var file = new FileStream(@"C:\Users\Eagle720\Documents\Visual Studio 2017\Projects\GeneratePDF\PdfReport_Iran\App_Data\test.txt",FileMode.Open,FileAccess.Read);

			for (int i = 0; i < file.Length; i++)
			{
				Console.WriteLine(file.ReadByte());
			}

			Console.ReadKey();
		}
	}
}
