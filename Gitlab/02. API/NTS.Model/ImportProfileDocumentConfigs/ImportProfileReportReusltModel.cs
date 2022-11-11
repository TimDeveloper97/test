using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.ImportProfileDocumentConfigs
{
    public class ImportProfileReportReusltModel
    {
        public string Id { get; set; }
        public int Step { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ProjectCode { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountVND { get; set; }
        public DateTime? DueDate { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierId { get; set; }
        public DateTime? PayDueDate { get; set; }
        public int PayStatus { get; set; }
        public int PayIndex { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }
        public string TransportSupplierName { get; set; }
        public string TransportSupplierId{ get; set; }
        public string CustomsSupplierName { get; set; }
        public string CustomsSupplierId { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TransportationInternationalCosts { get; set; }
        public int TransportationInternationalCostsUnit { get; set; }
        public decimal CustomsInlandCosts { get; set; }
        public decimal TransportExchangeRate { get; set; }
        public int ContractFinishStatus { get; set; }
        public int PayFinishStatus { get; set; }
        public int SupplierFinishStatus { get; set; }
        public int ProductionFinishStatus { get; set; }
        public int TransportFinishStatus { get; set; }
        public int CustomsFinishStatus { get; set; }
        public int WarehouseFinishStatus { get; set; }
    }
}