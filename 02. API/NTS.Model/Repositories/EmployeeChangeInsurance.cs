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
    
    public partial class EmployeeChangeInsurance
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string ReasonChangeInsuranceId { get; set; }
        public decimal InsuranceMoney { get; set; }
        public Nullable<System.DateTime> DateChange { get; set; }
        public string InsuranceLevelId { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
