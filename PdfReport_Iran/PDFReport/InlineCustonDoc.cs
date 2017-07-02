using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml.html.pdfelement;
using PdfReport_Iran.Models;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;


namespace PdfReport_Iran.PDFReport
{
	public class InlineCustonDoc
	{
		private readonly DBFirst _db = new DBFirst();

		public IPdfReportData CreatePdfReport()
		{
			return new PdfReport().DocumentPreferences(doc =>
				{
					doc.RunDirection(PdfRunDirection.RightToLeft);
					doc.Orientation(PageOrientation.Landscape);
					doc.PageSize(PdfPageSize.A4);
					doc.DocumentMetadata(new DocumentMetadata
					{
						Author = "Amin",
						Application = "Pdf_Iran",
						Keywords = "IList RPT.",
						Subject = "امتحان",
						Title = "Test"

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
					fonts.Path(System.IO.Path.Combine(AppPath.ApplicationPath, "fonts\\A.Afsaneh_p30download.com.ttf"),
						System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
					fonts.Size(13);
					fonts.Color(Color.Black);
				})
				.PagesFooter(footer =>
				{
					var date = PersianDate.ToPersianDateTime(DateTime.Now, "/", false);
					footer.InlineFooter(inlineFooter =>
					{
						inlineFooter.FooterProperties(new FooterBasicProperties
						{
							PdfFont = footer.PdfFont,
							HorizontalAlignment = HorizontalAlignment.Center,
							RunDirection = PdfRunDirection.RightToLeft,
							SpacingBeforeTable = 30,
							TotalPagesCountTemplateHeight = 9,
							TotalPagesCountTemplateWidth = 50
						});

						inlineFooter.AddPageFooter(data =>
						{
							return CreateFooter(footer, date, data);
						});
					});
				})
				.PagesHeader(header =>
				{
					header.CacheHeader(cache: true); // It's a default setting to improve the performance.
					header.XHtmlHeader(rptHeader =>
					{
						rptHeader.PageHeaderProperties(new XHeaderBasicProperties
						{
							RunDirection = PdfRunDirection.RightToLeft,
							ShowBorder = false,
						
						});
					});
					header.InlineHeader(inlineHeader =>
					{
						inlineHeader.AddPageHeader(data =>
						{
							var valDate = DateTime.Now.ToPersianDateTime(" / ", false);

							return CreateHeader(header, valDate);
						});
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
					var people = _db.People.OrderBy(p => p.Name).Include(g => g.Gender).ToList();

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
						column.HeaderCell("نام");
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<Person>(p => p.LastName);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(3);
						column.Width(3);
						column.HeaderCell("نام خانوادگی");
						column.Padding(5);
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<Person>(p => p.Age);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(4);
						column.Width(1);
						column.HeaderCell("سن");
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
					//export.ToCsv();
					//export.ToExcel();
					//export.ToXml();
				})
				.Generate(data => data.AsPdfFile($"{AppPath.ApplicationPath}\\Pdf\\Sample-{Guid.NewGuid():N}.pdf"));
		}
		private static PdfGrid CreateHeader(PagesHeaderBuilder header,string valDate)
		{
			var headerTitle = header;

			var table = new PdfGrid(numColumns: 3)
			{
				WidthPercentage = 100,
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				SpacingAfter = 10,
				
			};

			table.SetWidths(new int[]{33,33,33});
			//first cell
			PdfPTable tb1 = new PdfPTable(numColumns: 2)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL
			};
			tb1.DefaultCell.MinimumHeight = 30;
			var noMov = header.PdfFont.FontSelector.Process("شماره موقت:");
			var valMov = header.PdfFont.FontSelector.Process("123214");
			var noDae = header.PdfFont.FontSelector.Process("شماره دائم:");
			var valDae = header.PdfFont.FontSelector.Process("12324");
			var date = header.PdfFont.FontSelector.Process("تاریخ:");
			var datePhrase = header.PdfFont.FontSelector.Process(valDate);

			
			tb1.AddCell(new PdfPCell(noMov)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidth = 0,
				HorizontalAlignment = Element.ALIGN_RIGHT,
				ArabicOptions = 1
			});
			tb1.AddCell(new PdfPCell(valMov)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				Border = 0,
				HorizontalAlignment = Element.ALIGN_CENTER,
				PaddingLeft = 23,

			});

			
			tb1.AddCell(new PdfPCell(noDae)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidth = 0,
				HorizontalAlignment = Element.ALIGN_RIGHT,
				ArabicOptions = 1
			});
			tb1.AddCell(new PdfPCell(valDae)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				Border = 0,
				HorizontalAlignment = Element.ALIGN_CENTER,
				PaddingLeft = 23,

			});

			
			tb1.AddCell(new PdfPCell(date)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidth = 0,
				HorizontalAlignment = Element.ALIGN_RIGHT,
				ArabicOptions = 1
			});
			tb1.AddCell(new PdfPCell(datePhrase)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				Border = 0,
				HorizontalAlignment = Element.ALIGN_CENTER,
				PaddingLeft = 23,

			});


			table.AddCell(new PdfPTable(tb1));

			//second cell
			var headerTitle2 = headerTitle;
			headerTitle.PdfFont.Size = 25;
			headerTitle.PdfFont.Style = DocumentFontStyle.Bold;

			headerTitle2.PdfFont.Size = 40;
			headerTitle2.PdfFont.Style = DocumentFontStyle.Bold;

			PdfPTable tb2 = new PdfPTable(1)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
			};

			tb2.DefaultCell.MinimumHeight = 50;
			var nameCo = headerTitle.PdfFont.FontSelector.Process("نام شرکت");
			
			tb2.AddCell(new PdfPCell(nameCo)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidth = 0,
				HorizontalAlignment = Element.ALIGN_CENTER,
				Top = 25,
				
			});

			var nameDoc = headerTitle2.PdfFont.FontSelector.Process("نام سند");
			tb2.AddCell(new PdfPCell(nameDoc)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidth = 0,
				HorizontalAlignment = Element.ALIGN_CENTER,
				PaddingTop = 5
			});
			table.AddCell(new PdfPTable(tb2));

			//first cell
			PdfPTable tb3 = new PdfPTable(2)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
			};
			tb3.DefaultCell.MinimumHeight = 30;

			var docType = header.PdfFont.FontSelector.Process("نوع سند:");
			var valType = header.PdfFont.FontSelector.Process("123214");
			var docDesc = header.PdfFont.FontSelector.Process("شرح سند:");
			var valDesc = header.PdfFont.FontSelector.Process("12324");

			tb3.AddCell(new PdfPCell(docType)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidth = 0,
				HorizontalAlignment = Element.ALIGN_RIGHT,
				ArabicOptions = 1
			});
			tb3.AddCell(new PdfPCell(valType)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				Border = 0,
				HorizontalAlignment = Element.ALIGN_CENTER,
				PaddingLeft = 23,

			});


			tb3.AddCell(new PdfPCell(docDesc)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidth = 0,
				HorizontalAlignment = Element.ALIGN_RIGHT,
				ArabicOptions = 1
			});
			tb3.AddCell(new PdfPCell(valDesc)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				Border = 0,
				HorizontalAlignment = Element.ALIGN_CENTER,
				PaddingLeft = 23,

			});

			table.AddCell(new PdfPTable(tb3));



			return table;
		}

		private static PdfGrid CreateFooter(PagesFooterBuilder footer, string date, FooterData data)
		{
			var table = new PdfGrid(numColumns: 4)
			{
				WidthPercentage = 100,
				RunDirection = PdfWriter.RUN_DIRECTION_RTL
			};

			var datePhrase = footer.PdfFont.FontSelector.Process(date);
			var datePdfCell = new PdfPCell(datePhrase)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidthLeft = 0,
				BorderWidthRight = 0,
				BorderWidthTop = 1,
				BorderWidthBottom = 0,
				Padding = 4,
				BorderColorTop = new BaseColor(System.Drawing.Color.LightGray.ToArgb()),
				HorizontalAlignment = Element.ALIGN_CENTER
			};

			var nullPdfCell = new PdfPCell
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidthLeft = 0,
				BorderWidthRight = 0,
				BorderWidthTop = 1,
				BorderWidthBottom = 0,
				Padding = 4,
				BorderColorTop = new BaseColor(System.Drawing.Color.LightGray.ToArgb()),
				HorizontalAlignment = Element.ALIGN_RIGHT
			};

			var pageNumberPhrase = footer.PdfFont.FontSelector.Process("صفحه " + data.CurrentPageNumber + " از ");
			var pageNumberPdfCell = new PdfPCell(pageNumberPhrase)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidthLeft = 0,
				BorderWidthRight = 0,
				BorderWidthTop = 1,
				BorderWidthBottom = 0,
				Padding = 4,
				BorderColorTop = new BaseColor(System.Drawing.Color.LightGray.ToArgb()),
				HorizontalAlignment = Element.ALIGN_RIGHT
			};

			var totalPagesNumberImagePdfCell = new PdfPCell(data.TotalPagesCountImage)
			{
				RunDirection = PdfWriter.RUN_DIRECTION_RTL,
				BorderWidthLeft = 0,
				BorderWidthRight = 0,
				BorderWidthTop = 1,
				BorderWidthBottom = 0,
				Padding = 4,
				PaddingLeft = 0,
				BorderColorTop = new BaseColor(System.Drawing.Color.LightGray.ToArgb()),
				HorizontalAlignment = Element.ALIGN_LEFT
			};

			table.AddCell(datePdfCell);
			table.AddCell(nullPdfCell);
			table.AddCell(pageNumberPdfCell);
			table.AddCell(totalPagesNumberImagePdfCell);
			return table;
		}
	}
}