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
    
    public partial class RecruitmentRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RecruitmentRequest()
        {
            this.RecruitmentRequestAttaches = new HashSet<RecruitmentRequestAttach>();
        }
    
        public string Id { get; set; }
        public string WorkTypeId { get; set; }
        public string Code { get; set; }
        public string DepartmentId { get; set; }
        public int Quantity { get; set; }
        public System.DateTime RecruitmentDeadline { get; set; }
        public int Type { get; set; }
        public string RecruitmentReason { get; set; }
        public string Description { get; set; }
        public string Request { get; set; }
        public string Equipment { get; set; }
        public System.DateTime ApprovalDate { get; set; }
        public System.DateTime RequestDate { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public int Status { get; set; }
        public int Index { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public decimal MinCompanySalary { get; set; }
        public decimal MaxCompanySalary { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual WorkType WorkType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecruitmentRequestAttach> RecruitmentRequestAttaches { get; set; }
    }
}
