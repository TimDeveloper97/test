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
    
    public partial class ProductStandardTPAFile
    {
        public string Id { get; set; }
        public string ProductStandardTPAId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Nullable<decimal> FileSize { get; set; }
        public string Note { get; set; }
        public int Type { get; set; }
        public string DocumentTemplateId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ProductStandardTPA ProductStandardTPA { get; set; }
    }
}