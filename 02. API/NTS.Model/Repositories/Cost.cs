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
    
    public partial class Cost
    {
        public string Id { get; set; }
        public int Month { get; set; }
        public decimal Year { get; set; }
        public Nullable<decimal> EstimatedCost { get; set; }
        public Nullable<decimal> RealCost { get; set; }
    }
}