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
    
    public partial class DocumentProblem
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string Problem { get; set; }
        public int Status { get; set; }
        public System.DateTime ProblemDate { get; set; }
        public Nullable<System.DateTime> FinishExpectedDate { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual Document Document { get; set; }
    }
}
