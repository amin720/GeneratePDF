using System;
using System.Collections.Generic;
using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;

namespace PdfReport
{
	public class IListPdfReport
	{
		public IPdfReportData CreatePdfReport()
		{
			return new PdfRpt.FluentInterface.PdfReport().DocumentPreferences(doc =>
				{
					doc.RunDirection(PdfRunDirection.RightToLeft);
					doc.Orientation(PageOrientation.Portrait);
					doc.PageSize(PdfPageSize.A4);
					doc.DocumentMetadata(new DocumentMetadata
					{
						Author = "Vahid",
						Application = "PdfRpt",
						Keywords = "Test",
						Subject = "Test Rpt",
						Title = "Test"
					});
				})
				.DefaultFonts(fonts =>
				{
					fonts.Path(Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\tahoma.ttf",
						Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\verdana.ttf");
				})
				.PagesFooter(footer =>
				{
					footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
				})
				.PagesHeader(header =>
				{
					header.DefaultHeader(defaultHeader =>
					{
						defaultHeader.ImagePath(AppPath.ApplicationPath + "\\Images\\01.png");
						defaultHeader.Message("گزارش جدید ما");
					});
				})
				.MainTableTemplate(template =>
				{
					template.BasicTemplate(BasicTemplate.ClassicTemplate);
				})
				.MainTablePreferences(table =>
				{
					table.ColumnsWidthsType(TableColumnWidthType.Relative);
					table.NumberOfDataRowsPerPage(5);
				})
				.MainTableDataSource(dataSource =>
				{
					var listOfRows = new List<User>();
					for (int i = 0; i < 200; i++)
					{
						listOfRows.Add(new User {Id = i, LastName = "نام خانوادگی " + i, Name = "نام " + i, Balance = i + 1000});
					}
					dataSource.StronglyTypedList<User>(listOfRows);
				})
				.MainTableSummarySettings(summarySettings =>
				{
					summarySettings.OverallSummarySettings("جمع کل");
					summarySettings.PreviousPageSummarySettings("نقل از صفحه قبل");
					summarySettings.PageSummarySettings("جمع صفحه");
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
						column.HeaderCell("#");
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<User>(x => x.Id);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(1);
						column.Width(2);
						column.HeaderCell("شماره");
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<User>(x => x.Name);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(2);
						column.Width(3);
						column.HeaderCell("نام");
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<User>(x => x.LastName);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(3);
						column.Width(3);
						column.HeaderCell("نام خانوادگی");
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<User>(x => x.Balance);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(4);
						column.Width(2);
						column.HeaderCell("موجودی");
						column.ColumnItemsTemplate(template =>
						{
							template.TextBlock();
							template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
						});
						column.AggregateFunction(aggregateFunction =>
						{
							aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
							aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
						});
					});

				})
				.MainTableEvents(events =>
				{
					events.DataSourceIsEmpty(message: "رکوردی یافت نشد.");
				})
				.Export(export =>
				{
					export.ToExcel();
					export.ToCsv();
					export.ToXml();
				})
				.Generate(data => data.AsPdfFile(AppPath.ApplicationPath + "\\Pdf\\RptIListSample.pdf"));
		}
	}
}