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
    
    public partial class SolutionAttach
    {
        public string Id { get; set; }
        public string SolutionId { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string Path { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int Type { get; set; }
    
        public virtual Solution Solution { get; set; }
    }
}
