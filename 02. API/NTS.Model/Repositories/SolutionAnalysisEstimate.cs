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
    
    public partial class SolutionAnalysisEstimate
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string MaterialId { get; set; }
        public System.DateTime DeliveryTime { get; set; }
        public string Note { get; set; }
        public string MaterialGroupName { get; set; }
        public string MaterialGroupCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Pricing { get; set; }
    }
}