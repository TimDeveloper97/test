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
    
    public partial class EmployeeWorkHistory
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string TotalTime { get; set; }
        public string WorkTypeId { get; set; }
        public string ReferencePerson { get; set; }
        public string ReferencePersonPhone { get; set; }
        public Nullable<int> NumberOfManage { get; set; }
        public Nullable<decimal> Income { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
