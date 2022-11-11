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
    
    public partial class Interview
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Interview()
        {
            this.InterviewQuestions = new HashSet<InterviewQuestion>();
            this.InterviewUsers = new HashSet<InterviewUser>();
        }
    
        public string Id { get; set; }
        public string CandidateApplyId { get; set; }
        public string Name { get; set; }
        public System.DateTime InterviewDate { get; set; }
        public string InterviewBy { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<int> CorrectAnswer { get; set; }
        public string Comment { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDae { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string InterviewTime { get; set; }
        public int Index { get; set; }
    
        public virtual CandidateApply CandidateApply { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InterviewQuestion> InterviewQuestions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InterviewUser> InterviewUsers { get; set; }
    }
}