using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Drawing;
using PdfReport_Iran.Models;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfReport_Iran.PDFReport
{
	public class CustomHeaderDoc
	{
		private readonly DBFirst db = new DBFirst();
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
					footer.XHtmlFooter(rptFooter =>
					{
						rptFooter.PageFooterProperties(new XFooterBasicProperties
						{
							RunDirection = PdfRunDirection.RightToLeft,
							ShowBorder = true,
							PdfFont = footer.PdfFont,
							TotalPagesCountTemplateHeight = 10,
							TotalPagesCountTemplateWidth = 50,
							SpacingBeforeTable = 25f
						});

						rptFooter.AddPageFooter(pageFooter =>
						{
							var page = $"صفحه {pageFooter.CurrentPageNumber} از <TotalPagesNumber />";
							var date = PersianDate.ToPersianDateTime(DateTime.Now, "/", true);
							return $@"<table style='font-size:9pt;font-family:verdana;'>
														<tr>
															<td width='50%' align='center'>{page}</td>
															<td width='50%' align='center'>{date}</td>
														 </tr>
												</table>";
						});
					});
				})
				.PagesHeader(header =>
				{
					header.CacheHeader(cache: true);
					header.XHtmlHeader(rptHeader =>
					{
						rptHeader.PageHeaderProperties(new XHeaderBasicProperties
						{
							RunDirection = PdfRunDirection.RightToLeft,
							ShowBorder = false
						});
						rptHeader.AddPageHeader(pageHeader =>
						{
							var date = DateTime.Now.ToPersianDateTime(" / ", true);

							return $@"<table style='width:100%;direction:rtl;float:right'>
										<tr>
											<td colspan='2' style='width:33.333333%'>
												<table>
													<tr>
														<td style='padding: 5pt'>شماره موقت:</td>
														<td style='padding: 5pt'>11</td>
													</tr>
													<tr>
														<td style='padding: 5pt'>شماره دائم:</td>
														<td style='padding: 5pt'>22</td>
													</tr>
													<tr>
														<td style='padding: 5pt'>تاریخ سند:</td>
														<td style='padding: 5pt'>{date}</td>
													</tr>
												</table>
											</td>
											<td colspan='2' style='width:33.333333%;text-align: center;font-weight: bold;'>
												<table>
													<tr>
														<td style='padding: 5pt;font-size: 30pt;'>نام شرکت</td>
													</tr>
													<tr>
														<td style='padding: 5pt;font-size: 50pt;'>سند فلان</td>
													</tr>
												</table>
											</td>
											<td colspan='2' style='width:33.333333%'>
												<table>
													<tr>
														<td style='padding: 5pt'>نوع سند:</td>
														<td style='padding: 5pt'>66</td>
													</tr>
													<tr>
														<td style='padding: 5pt'>نوع سند:</td>
														<td style='padding: 5pt'>77</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>";	

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
					table.NumberOfDataRowsPerPage(10);
				})
				.MainTableDataSource(dataSource =>
				{
					var people = db.People.OrderBy(p => p.Name).Include(g => g.Gender).ToList();

					dataSource.StronglyTypedList(people);
				})
				.MainTableSummarySettings(summerySettings =>
				{
					summerySettings.OverallSummarySettings("جمع گروه");
					//summerySettings.PreviousPageSummarySettings("نقل از صفحه قبل");
					//summerySettings.PageSummarySettings("جمع كل گروه‌ها");
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
						column.HeaderCell("شماره ثبت شده");
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
				.Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\Sample-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
		}
	}
}