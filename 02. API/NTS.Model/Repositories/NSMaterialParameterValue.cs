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
    
    public partial class NSMaterialParameterValue
    {
        public string Id { get; set; }
        public string NSMaterialParameterId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual NSMaterialParameter NSMaterialParameter { get; set; }
    }
}
