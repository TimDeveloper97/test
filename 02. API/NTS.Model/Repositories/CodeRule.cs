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
    
    public partial class CodeRule
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string MaterialGroupId { get; set; }
        public string MaterialGroupTPAId { get; set; }
        public Nullable<int> Length { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string ManufactureId { get; set; }
        public string UnitId { get; set; }
        public string Type { get; set; }
    
        public virtual MaterialGroup MaterialGroup { get; set; }
        public virtual MaterialGroupTPA MaterialGroupTPA { get; set; }
    }
}
