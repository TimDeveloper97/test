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
    
    public partial class ProjectGeneralDesignModule
    {
        public string Id { get; set; }
        public string ProjectGeneralDesignId { get; set; }
        public string ProjectProductId { get; set; }
        public string ModuleId { get; set; }
        public decimal Quantity { get; set; }
        public decimal RealQuantity { get; set; }
        public decimal ErrorQuantity { get; set; }
        public decimal ContractPrice { get; set; }
        public decimal ModulePrice { get; set; }
        public string Note { get; set; }
        public int ModuleStatus { get; set; }
        public int Index { get; set; }
    
        public virtual ProjectGeneralDesign ProjectGeneralDesign { get; set; }
    }
}
