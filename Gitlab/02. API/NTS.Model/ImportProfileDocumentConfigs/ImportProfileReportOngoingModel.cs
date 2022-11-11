using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.ImportProfileDocumentConfigs
{
    public class ImportProfileReportOngoingModel 
    {
       public ImportProfileReportOngoingQuntityModel ImportProfile { get; set; }
        public List<ImportProfileReportOngoingQuntityModel> ImportProfileEmployee { get; set; }
        public List<ImportProfileReportOngoingQuntityModel> ImportProfileCustoms { get; set; }
        public List<ImportProfileReportOngoingQuntityModel> ImportProfileSupplier { get; set; }
        public List<ImportProfileReportOngoingQuntityModel> ImportProfileTransport { get; set; }
        public List<int> BarChartData { get; set; }
        public List<string> BarChartDataLabels { get; set; }
        public List<int> PieChartData { get; set; }

        public ImportProfileReportOngoingModel()
        {
            ImportProfileEmployee = new List<ImportProfileReportOngoingQuntityModel>();
            ImportProfileCustoms = new List<ImportProfileReportOngoingQuntityModel>();
            ImportProfileSupplier = new List<ImportProfileReportOngoingQuntityModel>();
            ImportProfileTransport = new List<ImportProfileReportOngoingQuntityModel>();
            ImportProfile = new ImportProfileReportOngoingQuntityModel();
            BarChartData = new List<int>();
            BarChartDataLabels = new List<string>();
            PieChartData = new List<int>();
        }
    }

    public class ImportProfileReportOngoingQuntityModel
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