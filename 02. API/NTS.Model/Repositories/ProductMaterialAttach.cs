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
    
    public partial class ProductMaterialAttach
    {
        public string Id { get; set; }
        public string ProductMaterialId { get; set; }
        public string FileName { get; set; }
        public Nullable<decimal> FileSize { get; set; }
        public string Path { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual ProductMaterial ProductMaterial { get; set; }
    }
}
