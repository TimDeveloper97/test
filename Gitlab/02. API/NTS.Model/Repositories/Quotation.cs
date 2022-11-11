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
    
    public partial class Quotation
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string Code { get; set; }
        public System.DateTime QuotationDate { get; set; }
        public int EffectiveLength { get; set; }
        public string EmployeeId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContactId { get; set; }
        public string Warranty { get; set; }
        public string Delivery { get; set; }
        public decimal QuotationPrice { get; set; }
        public int AdvanceRate { get; set; }
        public int SuccessRate { get; set; }
        public decimal ExpectedPrice { get; set; }
        public Nullable<System.DateTime> ImplementationDate { get; set; }
        public int Status { get; set; }
        public string SBUId { get; set; }
        public int Year { get; set; }
        public int AutoGenerateIndex { get; set; }
        public string Attachments { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
