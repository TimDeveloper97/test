using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.ImportProfileDocumentConfigs
{
    public class ImportProfileReportSummaryModel
    {
       public ImportProfileReportSummaryQuntityModel ImportProfile { get; set; }
        public List<ImportProfileReportSummaryQuntityModel> ImportProfileEmployee { get; set; }
        public List<ImportProfileReportSummaryQuntityModel> ImportProfileCustoms { get; set; }
        public List<ImportProfileReportSummaryQuntityModel> ImportProfileSupplier { get; set; }
        public List<ImportProfileReportSummaryQuntityModel> ImportProfileTransport { get; set; }
        public List<string> BarChartLabels { get; set; }
        public List<string> BarChartStepLabels { get; set; }
        public List<int> BarChartData { get; set; }
        public List<int> BarChartDataSlow { get; set; }
        public List<int> BarChartStepData { get; set; }
        public List<int> BarChartStepDataSlow { get; set; }
        public List<int> PieChartData { get; set; }

        public ImportProfileReportSummaryModel()
        {
            ImportProfileEmployee = new List<ImportProfileReportSummaryQuntityModel>();
            ImportProfileCustoms = new List<ImportProfileReportSummaryQuntityModel>();
            ImportProfileSupplier = new List<ImportProfileReportSummaryQuntityModel>();
            ImportProfileTransport = new List<ImportProfileReportSummaryQuntityModel>();
            ImportProfile = new ImportProfileReportSummaryQuntityModel();
            BarChartLabels = new List<string>();
            BarChartStepLabels = new List<string>();
            BarChartData = new List<int>();
            BarChartDataSlow = new List<int>();
            BarChartStepData = new List<int>();
            BarChartStepDataSlow = new List<int>();
            PieChartData = new List<int>();
        }
    }

    public class ImportProfileReportSummaryQuntityModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Total { get; set; }
        public int OngoingQuantity { get; set; }
        public int SlowQuantity { get; set; }
        public decimal PercentSlow { get; set; }
        public int SupplierQuantity { get; set; }
        public int ContractQuantity { get; set; }
        public int PayQuantity { get; set; }
        public int ProductionQuantity { get; set; }
        public int TranportQuantity { get; set; }
        public int CustomsQuantity { get; set; }
        public int WarehouseQuantity { get; set; }
        public decimal AmountVND { get; set; }
        public decimal CustomsInlandCosts { get; set; }
        public decimal TransportationInternationalCosts { get; set; }
    }
}