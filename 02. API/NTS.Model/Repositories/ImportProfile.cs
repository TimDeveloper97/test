//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NTS.Model.Repositories
{
    using System;
    using System.Collections.Generic;
    
    public partial class ImportProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ImportProfile()
        {
            this.ImportProfilePayments = new HashSet<ImportProfilePayment>();
            this.ImportProfileDocuments = new HashSet<ImportProfileDocument>();
            this.ImportProfileProducts = new HashSet<ImportProfileProduct>();
            this.ImportProfileDocumentOthers = new HashSet<ImportProfileDocumentOther>();
            this.ImportProfileQuotes = new HashSet<ImportProfileQuote>();
            this.ImportProfileTransportSuppliers = new HashSet<ImportProfileTransportSupplier>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Index { get; set; }
        public int Step { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public string PRCode { get; set; }
        public Nullable<System.DateTime> PRDueDate { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ManufacturerCode { get; set; }
        public decimal Amount { get; set; }
        public int ProcessingTime { get; set; }
        public string QuoteNumber { get; set; }
        public Nullable<System.DateTime> QuoteDate { get; set; }
        public string SupplierId { get; set; }
        public bool IsSupplier { get; set; }
        public decimal SupplierExchangeRate { get; set; }
        public Nullable<System.DateTime> SupplierExpectedDate { get; set; }
        public int SupplierFinishStatus { get; set; }
        public Nullable<System.DateTime> SupplierFinishDate { get; set; }
        public bool IsContract { get; set; }
        public string PONumber { get; set; }
        public Nullable<System.DateTime> ContractExpectedDate { get; set; }
        public int ContractFinishStatus { get; set; }
        public Nullable<System.DateTime> ContractFinishDate { get; set; }
        public int PayStatus { get; set; }
        public int PayIndex { get; set; }
        public Nullable<System.DateTime> PayDueDate { get; set; }
        public Nullable<System.DateTime> PayExpectedDate { get; set; }
        public int PayFinishStatus { get; set; }
        public Nullable<System.DateTime> PayFinishDate { get; set; }
        public Nullable<System.DateTime> EstimatedDeliveryDate { get; set; }
        public bool IsWarning { get; set; }
        public int WarningDay { get; set; }
        public Nullable<System.DateTime> ProductionExpectedDate { get; set; }
        public Nullable<System.DateTime> ProductionExpectedDate1 { get; set; }
        public Nullable<System.DateTime> ProductionExpectedDate2 { get; set; }
        public int ProductionFinishStatus { get; set; }
        public Nullable<System.DateTime> ProductionFinishDate { get; set; }
        public string TransportSupplierId { get; set; }
        public string TransportLeadtime { get; set; }
        public Nullable<decimal> ShippingCost { get; set; }
        public bool IsInsurrance { get; set; }
        public Nullable<decimal> TransportationCosts { get; set; }
        public decimal TransportationInternationalCosts { get; set; }
        public int TransportationInternationalCostsUnit { get; set; }
        public string PackageQuantity { get; set; }
        public string PackingSize { get; set; }
        public string NetWeight { get; set; }
        public decimal TransportExchangeRate { get; set; }
        public Nullable<System.DateTime> TransportExpectedDate { get; set; }
        public int TransportFinishStatus { get; set; }
        public Nullable<System.DateTime> TransportFinishDate { get; set; }
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
        public Nullable<System.DateTime> CustomsDeclarationFormDate { get; set; }
        public Nullable<System.DateTime> CustomsClearanceFromDate { get; set; }
        public string CustomsTypeCode { get; set; }
        public string CustomsSupplierId { get; set; }
        public decimal CustomsInlandCosts { get; set; }
        public Nullable<System.DateTime> CustomExpectedDate { get; set; }
        public int CustomsFinishStatus { get; set; }
        public Nullable<System.DateTime> CustomsFinishDate { get; set; }
        public bool WarehouseStatus { get; set; }
        public string WarehouseCode { get; set; }
        public Nullable<System.DateTime> WarehouseDate { get; set; }
        public Nullable<System.DateTime> WarehouseExpectedDate { get; set; }
        public Nullable<System.DateTime> WarehouseFinishDate { get; set; }
        public int WarehouseFinishStatus { get; set; }
        public string EmployeeId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public decimal OtherCosts { get; set; }
        public string DeliveryConditions { get; set; }
        public int CurrencyUnit { get; set; }
        public string ChargingWeight { get; set; }
        public Nullable<System.DateTime> CurrentExpected { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportProfilePayment> ImportProfilePayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportProfileDocument> ImportProfileDocuments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportProfileProduct> ImportProfileProducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportProfileDocumentOther> ImportProfileDocumentOthers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportProfileQuote> ImportProfileQuotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportProfileTransportSupplier> ImportProfileTransportSuppliers { get; set; }
    }
}
