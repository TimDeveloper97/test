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
    
    public partial class SupplierManufacture
    {
        public string Id { get; set; }
        public string SupplierId { get; set; }
        public string ManufactureId { get; set; }
    
        public virtual Manufacture Manufacture { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
