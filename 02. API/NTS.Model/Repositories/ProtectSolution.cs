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
    
    public partial class ProtectSolution
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string SolutionId { get; set; }
        public string Request { get; set; }
        public string Solution { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string Note { get; set; }
    
        public virtual CustomerRequirement CustomerRequirement { get; set; }
    }
}
