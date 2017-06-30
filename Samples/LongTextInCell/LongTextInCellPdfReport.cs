﻿using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;

namespace PdfReportSamples.LongTextInCell
{
    public class LongTextInCellPdfReport
    {
        public IPdfReportData CreatePdfReport()
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Vahid", Application = "PdfRpt", Keywords = "IList Rpt.", Subject = "Test Rpt", Title = "Test" });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
            })
            .DefaultFonts(fonts =>
            {
                fonts.Path(System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
                           System.IO.Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
                fonts.Size(9);
                fonts.Color(System.Drawing.Color.Black);
            })
            .PagesFooter(footer =>
            {
                footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
            })
            .PagesHeader(header =>
            {
                header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                header.DefaultHeader(defaultHeader =>
                {
                    defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                    defaultHeader.ImagePath(System.IO.Path.Combine(AppPath.ApplicationPath, "Images\\01.png"));
                    defaultHeader.Message("Our new rpt.");
                });
            })
            .MainTableTemplate(template =>
            {
                template.BasicTemplate(BasicTemplate.ClassicTemplate);
            })
            .MainTablePreferences(table =>
            {
                table.ColumnsWidthsType(TableColumnWidthType.Relative);
                //table.NumberOfDataRowsPerPage(50); // don't set this line and let the PdfRpt calculate it automatically.
                //for long texts larger than a page
                table.SplitLate(false);
                table.SplitRows(true);
            })
            .MainTableDataSource(dataSource =>
            {
                var longText = string.Join("", Enumerable.Repeat("a", 1500).ToArray());
                var report = new ApprovalReport
                {
                    DocumentTitle = "test",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    InstanceId = Guid.Empty,
                    WorkflowInitiator = "test",
                    WorkflowInitiatorUrl = "test",
                    Approvals = new List<ApprovalReport.Approval>
                    {
                        new ApprovalReport.Approval{ Number = "1", ApprovalDate = DateTime.Now, Approver = "test1", 
                            ApproverUrl = "test", Department ="test", Position  = "test", Result = "test", Url = "test", Commentary = "test"},

                        new ApprovalReport.Approval{ Number = "2", ApprovalDate = DateTime.Now, Approver = "test2", 
                            ApproverUrl = "test", Department ="test", Position  = "test", Result = "test", Url = "test",                                                     
                            Commentary = longText },

                        new ApprovalReport.Approval{ Number = "3", ApprovalDate = DateTime.Now, Approver = "test3", 
                            ApproverUrl = "test", Department ="test", Position  = "test", Result = "test", Url = "test", Commentary = "test"},
                    }
                };

                dataSource.StronglyTypedList(report.Approvals);
            })
            .MainTableSummarySettings(summarySettings =>
            {
                summarySettings.OverallSummarySettings("Summary");
                summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                summarySettings.PageSummarySettings("Page Summary");
            })
            .MainTableColumns(columns =>
            {
                columns.AddColumn(column =>
                {
                    column.PropertyName<ApprovalReport.Approval>(x => x.Number);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(1);
                    column.Width(1);
                    column.HeaderCell("№ П/П");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<ApprovalReport.Approval>(x => x.Approver);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(2);
                    column.Width(3);
                    column.HeaderCell("Ф.И.О. согласующего");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<ApprovalReport.Approval>(x => x.Department);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(3);
                    column.Width(3);
                    column.HeaderCell("Подразделение");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<ApprovalReport.Approval>(x => x.Position);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(4);
                    column.Width(3);
                    column.HeaderCell("Должность");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<ApprovalReport.Approval>(x => x.ApprovalDate);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(5);
                    column.Width(2);
                    column.HeaderCell("Дата согласования");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<ApprovalReport.Approval>(x => x.Result);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(6);
                    column.Width(2);
                    column.HeaderCell("Результат");
                });

                columns.AddColumn(column =>
                {
                    column.PropertyName<ApprovalReport.Approval>(x => x.Commentary);
                    column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                    column.IsVisible(true);
                    column.Order(7);
                    column.Width(4);
                    column.HeaderCell("Замечания/комментарии");
                });
            })
            .MainTableEvents(events =>
            {
                events.DataSourceIsEmpty(message: "Нет данных для отображения.");

                events.MainTableCreated(args =>
                {
                    var infoTable = new PdfGrid(numColumns: 1)
                    {
                        WidthPercentage = 100
                    };
                    infoTable.AddSimpleRow((cellData, properties) =>
                    {
                        properties.HorizontalAlignment = HorizontalAlignment.Left;
                        cellData.Value = string.Format("Название документа:   {0}", "DocumentTitle");
                        properties.PdfFont = events.PdfFont;
                        properties.RunDirection = PdfRunDirection.LeftToRight;
                    });
                    infoTable.AddSimpleRow((cellData, properties) =>
                    {
                        properties.HorizontalAlignment = HorizontalAlignment.Left;
                        cellData.Value = string.Format("Инициатор согласования:   {0}", "WorkflowInitiator");
                        properties.PdfFont = events.PdfFont;
                        properties.RunDirection = PdfRunDirection.LeftToRight;
                    });
                    infoTable.AddSimpleRow((cellData, properties) =>
                    {
                        properties.HorizontalAlignment = HorizontalAlignment.Left;
                        cellData.Value = string.Format("Дата начала согласования:   {0}", "StartDate");
                        properties.PdfFont = events.PdfFont;
                        properties.RunDirection = PdfRunDirection.LeftToRight;
                    });
                    infoTable.AddSimpleRow((cellData, properties) =>
                    {
                        properties.HorizontalAlignment = HorizontalAlignment.Left;
                        cellData.Value = string.Format("Дата окончания согласования:   {0}", "EndDate");
                        properties.PdfFont = events.PdfFont;
                        properties.RunDirection = PdfRunDirection.LeftToRight;
                    });

                    var table = infoTable.AddBorderToTable(borderColor: BaseColor.LIGHT_GRAY, spacingBefore: 10f);
                    table.SpacingAfter = 10f;
                    args.PdfDoc.Add(table);
                });
            })
            .Export(export =>
            {
                export.ToExcel();
                export.ToCsv();
                export.ToXml();
            })
            .Generate(data => data.AsPdfFile(string.Format("{0}\\Pdf\\RptLongTextInCellSample-{1}.pdf", AppPath.ApplicationPath, Guid.NewGuid().ToString("N"))));
        }
    }
}