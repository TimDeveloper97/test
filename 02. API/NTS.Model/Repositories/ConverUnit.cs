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
    
    public partial class ConverUnit
    {
        public string Id { get; set; }
        public string UnitId { get; set; }
        public string MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public decimal ConvertQuantity { get; set; }
        public decimal LossRate { get; set; }
    
        public virtual Material Material { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
