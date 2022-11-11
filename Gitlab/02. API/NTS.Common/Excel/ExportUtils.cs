using Syncfusion.ExcelToPdfConverter;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NTS.Common.Excel
{
    public static class ExportUtils
    {
        public static string ExportExcel<T>(List<T> datas, string templateCode, string searchTitle, string conditionColumn, int columns, bool hasFooter, string fileName, string titleTotal = "")
        {
            // Khỏi tạo bảng excel
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;
            IWorkbook workbook = ExportData(application, datas, templateCode, searchTitle, conditionColumn, columns, hasFooter, false, titleTotal, fileName);

            string pathExport = "/Template/Export/" + new DateTimeOffset(DateTime.UtcNow) + ".xlsx";
            workbook.SaveAs(HttpContext.Current.Server.MapPath(pathExport));

            workbook.Close();
            excelEngine.Dispose();

            return pathExport;
        }

        public static string ExportPdf<T>(List<T> datas, string templateCode, string searchTitle, string conditionColumn, int columns, bool hasFooter, string fileName, string titleTotal = "")
        {
            // Khỏi tạo bảng excel
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;
            IWorkbook workbook = ExportData(application, datas, templateCode, searchTitle, conditionColumn, columns, hasFooter, true, titleTotal, fileName);

            string pathExport = "/Template/Export/" + new DateTimeOffset(DateTime.UtcNow) + ".pdf";

            ExcelToPdfConverter converter = new ExcelToPdfConverter(workbook.Worksheets[0]);
            PdfDocument pdfDocument = converter.Convert();
            pdfDocument.Save(HttpContext.Current.Server.MapPath(pathExport));
            pdfDocument.Close();
            converter.Dispose();

            workbook.Close();
            excelEngine.Dispose();

            return pathExport;
        }

        public static void SetStyleExcel(IWorkbook workbook, int countData, int columns, bool isAutoFitRows)
        {
            IWorksheet sheet = workbook.Worksheets[0];
            int rowData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase).Row;

            string columnName = GetExcelColumnName(columns);
            if (isAutoFitRows)
            {
                sheet.Range["A" + rowData + ":" + columnName + (rowData + countData)].AutofitRows();
            }

            IStyle bodyStyle = workbook.Styles.Add("BodyStyle");

            bodyStyle.BeginUpdate();
            bodyStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            bodyStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            bodyStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            bodyStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;

            bodyStyle.EndUpdate();

            sheet.Range["A" + rowData + ":" + columnName + (rowData + countData)].CellStyleName = "BodyStyle";
        }

        private static IWorkbook ExportData<T>(IApplication application, List<T> datas, string templateCode, string searchTitle, string conditionColumn, int columns, bool hasFooter, bool autofitRows, string titleTotal, string fileName)
        {
            IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath("/" + fileName));
            IWorksheet sheet = workbook.Worksheets[0];

            int total = datas.Count;
            int rowData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase).Row;

            sheet.ImportData(datas, rowData, 1, false);

            // Autofit dòng đầu tiên của Data khi import, thường khi xuất Pdf thì dòng đầu tiên đang không autofit
            string columnName = GetExcelColumnName(columns);
            if (autofitRows)
            {
                sheet.Range["A" + rowData + ":" + columnName + (rowData + total)].AutofitRows();
            }

            IStyle bodyStyle = workbook.Styles.Add("BodyStyle");

            bodyStyle.BeginUpdate();
            bodyStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            bodyStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            bodyStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            bodyStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;

            bodyStyle.EndUpdate();

            sheet.Range["A" + rowData + ":" + columnName + (rowData + total)].CellStyleName = "BodyStyle";

            return workbook;
        }

        public static void ExportToChartPie(IWorksheet sheet, int topRow, int leftColumn,int rightColumn,int bottomRow, object[] xValues, object[] yValues)
        {
            IChartShape chart = sheet.Charts.Add();
            IChartSerie serie = chart.Series.Add(ExcelChartType.Pie);

            serie.EnteredDirectlyValues = yValues;
            serie.EnteredDirectlyCategoryLabels = xValues;

            chart.TopRow = topRow;
            chart.LeftColumn = leftColumn;
            chart.RightColumn = rightColumn;
            chart.BottomRow = bottomRow;
        }

        public static void ExportToChartBar(IWorksheet sheet, int topRow, int leftColumn, int rightColumn, int bottomRow, string name, object[] xValues, object[] yValues)
        {
            IChartShape chart = sheet.Charts.Add();
            chart.ChartType = ExcelChartType.Column_Clustered;

            IChartSerie serie1 = chart.Series.Add(name);
            serie1.EnteredDirectlyValues = yValues;
            serie1.EnteredDirectlyCategoryLabels = xValues;

            chart.TopRow = topRow;
            chart.LeftColumn = leftColumn;
            chart.RightColumn = rightColumn;
            chart.BottomRow = bottomRow;
        }

        public static void ExportToChartBar(IWorksheet sheet, int topRow, int leftColumn, int rightColumn, int bottomRow, object[] xValues,string name1, object[] yValues1, string name2, object[] yValues2)
        {
            IChartShape chart = sheet.Charts.Add();
            chart.ChartType = ExcelChartType.Column_Clustered;
           
            IChartSerie serie1 = chart.Series.Add(name1);
            serie1.EnteredDirectlyValues = yValues1;
            serie1.EnteredDirectlyCategoryLabels = xValues;

            IChartSerie serie2 = chart.Series.Add(name2);
            serie2.EnteredDirectlyValues = yValues2;
            serie2.EnteredDirectlyCategoryLabels = xValues;

            chart.TopRow = topRow;
            chart.LeftColumn = leftColumn;
            chart.RightColumn = rightColumn;
            chart.BottomRow = bottomRow;
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
