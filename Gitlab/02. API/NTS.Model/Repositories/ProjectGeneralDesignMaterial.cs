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
    
    public partial class ProjectGeneralDesignMaterial
    {
        public string Id { get; set; }
        public string ProjectGeneralDesignId { get; set; }
        public string MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Inventoty { get; set; }
        public decimal ContractPrice { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public int Type { get; set; }
    
        public virtual ProjectGeneralDesign ProjectGeneralDesign { get; set; }
    }
}
