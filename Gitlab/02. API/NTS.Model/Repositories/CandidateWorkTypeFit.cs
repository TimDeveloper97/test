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
    
    public partial class CandidateWorkTypeFit
    {
        public string Id { get; set; }
        public string WorkTypeId { get; set; }
        public string CandidateId { get; set; }
    
        public virtual Candidate Candidate { get; set; }
    }
}
