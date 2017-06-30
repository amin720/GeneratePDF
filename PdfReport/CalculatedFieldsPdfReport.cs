using System;
using System.Collections.Generic;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfReport
{
	public class CalculatedFieldsPdfReport
	{
		public IPdfReportData CreatePdfReport()
		{
			return new PdfRpt.FluentInterface.PdfReport().DocumentPreferences(doc =>
				{
					doc.RunDirection(PdfRunDirection.RightToLeft);
					doc.Orientation(PageOrientation.Portrait);
					doc.PageSize(PdfPageSize.A4);
					doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "Test", Subject = "Test Rpt", Title = "Test" });
				})
				.DefaultFonts(fonts =>
				{
					fonts.Path(AppPath.ApplicationPath + "\\fonts\\irsans.ttf",
						Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\verdana.ttf");
				})
				.PagesFooter(footer =>
				{
					footer.DefaultFooter(printDate: DateTime.Now.ToString("MM/dd/yyyy"));
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
					template.BasicTemplate(BasicTemplate.RainyDayTemplate);
				})
				.MainTablePreferences(table =>
				{
					table.ColumnsWidthsType(TableColumnWidthType.Relative);
				})
				.MainTableDataSource(dataSource =>
				{
					var listOfRows = new List<User>();
					for (int i = 0; i < 220; i++)
					{
						listOfRows.Add(new User { Id = i, LastName = "نام خانوادگی " + i, Name = "نام " + i, Balance = i + 1000 });
					}
					dataSource.StronglyTypedList<User>(listOfRows);
				})
				.MainTableEvents(events =>
				{
					events.DataSourceIsEmpty(message: "رکوردی یافت نشد.");
				})
				.MainTableSummarySettings(summary =>
				{
					summary.OverallSummarySettings("جمع");
					summary.PreviousPageSummarySettings("نقل از صفحه قبل");
					summary.PageSummarySettings("جمع صفحه");
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
						column.HeaderCell("ردیف", captionRotation: 90);
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
						column.Width(2);
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
						column.PropertyName("CF1");
						column.CalculatedField(true,
							list =>
							{
								if (list == null) return string.Empty;
								var name = list.GetSafeStringValueOf<User>(x => x.Name);
								var lastName = list.GetSafeStringValueOf<User>(x => x.LastName);
								return name + " - " + lastName;
							});
						column.HeaderCell("ف.م.");
						column.Width(3);
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(4);
					});

					columns.AddColumn(column =>
					{
						column.PropertyName<User>(x => x.Balance);
						column.HeaderCell("موجودی");
						column.ColumnItemsTemplate(template =>
						{
							template.TextBlock();
							template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
						});
						column.Width(2);
						column.AggregateFunction(aggregateFunction =>
						{
							aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
							aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
						});
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.IsVisible(true);
						column.Order(5);
					});

					columns.AddColumn(column =>
					{
						column.PropertyName("CF2");
						column.HeaderCell("ف.م.");
						column.Width(3);
						column.AggregateFunction(aggregateFunction =>
						{
							aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
							aggregateFunction.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
						});
						column.CellsHorizontalAlignment(HorizontalAlignment.Center);
						column.ColumnItemsTemplate(template =>
						{
							template.TextBlock();
							template.DisplayFormatFormula(obj => obj == null ? string.Empty : string.Format("{0:n0}", obj));
						});
						column.CalculatedField(true,
							list =>
							{
								if (list == null) return string.Empty;
								var balance = list.GetValueOf<User>(x => x.Balance);
								return (long)balance * 3;
							});
						column.IsVisible(true);
						column.Order(6);
					});
				})
				.Export(export =>
				{
					export.ToExcel(footer: "Footer text", header: "&24&U&\"Arial,Regular Bold\" New rpt.", pageLayoutView: true);
					export.ToXml();
				})
				.Generate(data => data.AsPdfFile(AppPath.ApplicationPath + "\\Pdf\\RptCalculatedFieldsSample.pdf"));
		}
	}
}
