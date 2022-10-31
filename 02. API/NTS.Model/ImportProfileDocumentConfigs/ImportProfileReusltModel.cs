using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.ImportProfileDocumentConfigs
{
    public class ImportProfileReusltModel
    {
        public string Id { get; set; }
        public int Step { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ManufacturerCode { get; set; }
        public string PRCode { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountVND { get; set; }
        public DateTime? PRDueDate { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierId { get; set; }
        public DateTime? PayDueDate { get; set; }
        public int PayWarning { get; set; }
        public int PayStatus { get; set; }
        public int PayIndex { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }
        public int SupplierFinishStatus { get; set; }
        public int ContractFinishStatus { get; set; }
        public int PayFinishStatus { get; set; }
        public int ProductionFinishStatus { get; set; }
        public int TransportFinishStatus { get; set; }
        public int CustomsFinishStatus { get; set; }
        public int WarehouseFinishStatus { get; set; }
        public int StepStatus { get; set; }
        public int CurrencyUnit { get; set; }
        public DateTime? CurrentExpected { get; set; }
        public DateTime? SupplierExpectedDate { get; set; }
        public string SupplierExpectedDateDay { get; set; }
        public DateTime? ContractExpectedDate { get; set; }
        public string ContractExpectedDateDay { get; set; }
        public DateTime? PayExpectedDate { get; set; }
        public string PayExpectedDateDay { get; set; }
        public DateTime? ProductionExpectedDate { get; set; }
        public string ProductionExpectedDateDay { get; set; }
        public DateTime? TransportExpectedDate { get; set; }
        public string TransportExpectedDateDay { get; set; }
        public DateTime? CustomExpectedDate { get; set; }
        public string CustomExpectedDateDay { get; set; }
        public DateTime? WarehouseExpectedDate { get; set; }
        public string WarehouseExpectedDateDay { get; set; }
        public int ProblemExistQuantity { get; set; }
        public int ProductProgress { get; set; }
    }
}