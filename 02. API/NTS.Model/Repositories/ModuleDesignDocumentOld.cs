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
    
    public partial class ModuleDesignDocumentOld
    {
        public string Id { get; set; }
        public string ModuleOldVersionId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ServerPath { get; set; }
        public decimal FileSize { get; set; }
        public string FileType { get; set; }
        public string ParentId { get; set; }
        public int DesignType { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string HashValue { get; set; }
    }
}
