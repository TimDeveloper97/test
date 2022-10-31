using NTS.Model.ImportPR;
using NTS.Model.ImportProfileProblemExist;
using NTS.Model.ImportProfileTransportSupplier;
using System;
using System.Collections.Generic;
namespace NTS.Model.ImportProfile
{
    public class ImportProfileUpdateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Step { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public string PRCode { get; set; }
        public DateTime? PRDueDate { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ManufacturerCode { get; set; }
        public decimal Amount { get; set; }
        public int ProcessingTime { get; set; }
        public string QuoteNumber { get; set; }
        public DateTime? QuoteDate { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public bool IsSupplier { get; set; }
        public DateTime? SupplierFinishDate { get; set; }
        public bool IsContract { get; set; }
        public string PONumber { get; set; }
        public DateTime? ContractFinishDate { get; set; }
        public int PayStatus { get; set; }
        public int PayIndex { get; set; }
        public DateTime? PayDueDate { get; set; }
        public DateTime? PayFinishDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public bool IsWarning { get; set; }
        public int WarningDay { get; set; }
        public DateTime? ProductionFinishDate { get; set; }
        public string TransportSupplierId { get; set; }
        public string TransportSupplierName { get; set; }
        public string TransportLeadtime { get; set; }
        public decimal? ShippingCost { get; set; }
        public bool IsInsurrance { get; set; }
        public decimal? TransportationCosts { get; set; }
        public DateTime? TransportFinishDate { get; set; }
        public string CustomsName { get; set; }
        public bool CustomsElearanceStatus { get; set; }
        public decimal CustomsElearanceValue { get; set; }
        public string CustomsType { get; set; }
        public string CustomsNote { get; set; }
        public decimal VAT { get; set; }
        public decimal ExportTax { get; set; }
        public string HSCode { get; set; }
        public decimal ImportPercent { get; set; }
        public string CustomsDeclarationFormCode { get; set; }
        public DateTime? CustomsDeclarationFormDate { get; set; }
        public DateTime? CustomsClearanceFromDate { get; set; }
        public string CustomsSupplierId { get; set; }
        public string CustomsSupplierName { get; set; }
        public string CustomsTypeCode { get; set; }
        public decimal CustomsInlandCosts { get; set; }
        public DateTime? CustomsFinishDate { get; set; }
        public bool WarehouseStatus { get; set; }
        public string WarehouseCode { get; set; }
        public DateTime? WarehouseDate { get; set; }
        public DateTime? WarehouseFinishDate { get; set; }
        public string EmployeeId { get; set; }
        public decimal TransportationInternationalCosts { get; set; }
        public int TransportationInternationalCostsUnit { get; set; }
        public string PackageQuantity { get; set; }
        public string PackingSize { get; set; }
        public string NetWeight { get; set; }
        public string ChargingWeight { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? SupplierExpectedDate { get; set; }
        public DateTime? ContractExpectedDate { get; set; }
        public DateTime? PayExpectedDate { get; set; }
        public DateTime? ProductionExpectedDate { get; set; }
        public DateTime? ProductionExpectedDate1 { get; set; }
        public DateTime? ProductionExpectedDate2 { get; set; }
        public DateTime? TransportExpectedDate { get; set; }
        public DateTime? CustomExpectedDate { get; set; }
        public DateTime? WarehouseExpectedDate { get; set; }
        public int SupplierFinishStatus { get; set; }
        public int ContractFinishStatus { get; set; }
        public int PayFinishStatus { get; set; }
        public int ProductionFinishStatus { get; set; }
        public int TransportFinishStatus { get; set; }
        public int CustomFinishStatus { get; set; }
        public int WarehouseFinishStatus { get; set; }
        public decimal SupplierExchangeRate { get; set; }
        public decimal TransportExchangeRate { get; set; }
        public decimal OtherCosts { get; set; }
        public string DeliveryConditions { get; set; }
        public int CurrencyUnit { get; set; }
        public List<ImportProfilePaymentModel> ListPayment { get; set; }
        public List<ImportProfileQuoteModel> ListDocumentStep1 { get; set; }
        public List<ImportProfileDocumentModel> ListDocumentStep2 { get; set; }
        public List<ImportProfileDocumentModel> ListDocumentStep3 { get; set; }
        public List<ImportProfileDocumentModel> ListDocumentStep4 { get; set; }
        public List<ImportProfileDocumentModel> ListDocumentStep5 { get; set; }
        public List<ImportProfileDocumentModel> ListDocumentStep6 { get; set; }
        public List<ImportProfileDocumentModel> ListDocumentStep7 { get; set; }
        public List<ImportProfileDocumentOtherModel> ListDocumentOtherStep1 { get; set; }
        public List<ImportProfileDocumentOtherModel> ListDocumentOtherStep2 { get; set; }
        public List<ImportProfileDocumentOtherModel> ListDocumentOtherStep3 { get; set; }
        public List<ImportProfileDocumentOtherModel> ListDocumentOtherStep4 { get; set; }
        public List<ImportProfileDocumentOtherModel> ListDocumentOtherStep5 { get; set; }
        public List<ImportProfileDocumentOtherModel> ListDocumentOtherStep6 { get; set; }
        public List<ImportProfileDocumentOtherModel> ListDocumentOtherStep7 { get; set; }
        public List<ImportPRUpdateModel> ListMaterial { get; set; }
        public List<ImportProfileProblemExistCreateModel> ListProblem { get; set; }
        public List<ImportProfileTransportSupplierModel> ListTransportSupplier { get; set; }
        public ImportProfileUpdateModel()
        {
            ListDocumentStep1 = new List<ImportProfileQuoteModel>();
            ListDocumentStep2 = new List<ImportProfileDocumentModel>();
            ListDocumentStep3 = new List<ImportProfileDocumentModel>();
            ListDocumentStep4 = new List<ImportProfileDocumentModel>();
            ListDocumentStep5 = new List<ImportProfileDocumentModel>();
            ListDocumentStep6 = new List<ImportProfileDocumentModel>();
            ListDocumentStep7 = new List<ImportProfileDocumentModel>();
            ListDocumentOtherStep1 = new List<ImportProfileDocumentOtherModel>();
            ListDocumentOtherStep2 = new List<ImportProfileDocumentOtherModel>();
            ListDocumentOtherStep3 = new List<ImportProfileDocumentOtherModel>();
            ListDocumentOtherStep4 = new List<ImportProfileDocumentOtherModel>();
            ListDocumentOtherStep5 = new List<ImportProfileDocumentOtherModel>();
            ListDocumentOtherStep6 = new List<ImportProfileDocumentOtherModel>();
            ListDocumentOtherStep7 = new List<ImportProfileDocumentOtherModel>();
            ListMaterial = new List<ImportPRUpdateModel>();
            ListPayment = new List<ImportProfilePaymentModel>();
            ListProblem = new List<ImportProfileProblemExistCreateModel>();
            ListTransportSupplier = new List<ImportProfileTransportSupplierModel>();
        }
    }
}
