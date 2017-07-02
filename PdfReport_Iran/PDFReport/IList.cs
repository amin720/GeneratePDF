using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using PdfReport_Iran.Models;
using PdfRpt.Core;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;

namespace PdfReport_Iran.PDFReport
{
	public class IList
	{
		private readonly DBFirst db = new DBFirst();
		public IPdfReportData CreatePdfReport()
		{
			return new PdfReport().DocumentPreferences(doc =>
				{
					doc.RunDirection(PdfRunDirection.LeftToRight);
					doc.Orientation(PageOrientation.Portrait);
					doc.PageSize(PdfPageSize.A4);
					doc.DocumentMetadata(new DocumentMetadata
					{
						Author = "Amin",Application = "Pdf_Iran",Keywords = "IList RPT.",Subject = "امتحان",Title = "Test"
						
					});
					doc.Compression(new CompressionSettings
					{
						EnableCompression = true,
						EnableFullCompression = true
					});
					doc.PrintingPreferences(new PrintingPreferences
					{
						ShowPrintDialogAutomatically = true
					});
				})
				.DefaultFonts(fonts =>
				{
					fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
								System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
					fonts.Size(13);
					fonts.Color(Color.Black);
				})
				.PagesFooter(footer =>
				{
					footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
				})
				.PagesHeader(header =>
				{
					header.CacheHeader(cache: true);
					header.DefaultHeader(defualtHeader =>
					{
						defualtHeader.RunDirection(PdfRunDirection.LeftToRight);
						defualtHeader.Message("این یک سند تست می باشد");
					});
				})
				.MainTableTemplate(template =>
				{
					template.BasicTemplate(BasicTemplate.MochaTemplate);
				})
				.MainTablePreferences(table =>
				{
					table.ColumnsWidthsType(TableColumnWidthType.Relative);
					table.NumberOfDataRowsPerPage(15);
				})
				.MainTableDataSource(dataSource =>
				{
					var people = db.People.OrderBy(p => p.Name).Include(g => g.Gender).ToList();

					dataSource.StronglyTypedList(people);
				})
				.MainTableSummarySettings(summerySettings =>
				{
					summerySettings.OverallSummarySettings("خلاصه");
					summerySettings.PreviousPageSummarySettings("Previous Page Summary");
					summerySettings.PageSummarySettings("Page Summary");
				})
				.MainTableColumns(columns =>
				{
					columns.AddColumn(column =>
					{
						column.PropertyName("rowNo");
						column.IsRowNumber(true);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(0);
						column.Width(1);
						column.HeaderCell("ردیف");
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<Person>(p => p.Id);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(1);
						column.Width(1);
						column.HeaderCell("Id");
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<Person>(p => p.FirstName);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(2);
						column.Width(2);
						column.HeaderCell("First Name", horizontalAlignment: HorizontalAlignment.Left);
						column.Font(font =>
						{
							font.Size(11);
							font.Color(Color.BlueViolet);
						});
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<Person>(p => p.LastName);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(3);
						column.Width(3);
						column.HeaderCell("Last Name");
						column.Padding(5);
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<Person>(p => p.Age);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(4);
						column.Width(1);
						column.HeaderCell("Age");
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<Person>(p => p.Gender.Name);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(5);
						column.Width(2);
						column.HeaderCell("جنسیت");
					});
				})
				.MainTableEvents(events =>
				{
					events.DataSourceIsEmpty(message: "هیچ داده ای برای نمایش وجود ندارد");
				})
				.Export(export =>
				{
					export.ToCsv();
					export.ToExcel();
					export.ToXml();
				})
				.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\Sample-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
		}
	}
}