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
    
    public partial class MaterialImage
    {
        public string Id { get; set; }
        public string MaterialId { get; set; }
        public string Path { get; set; }
        public string ThumbnailPath { get; set; }
        public string FileName { get; set; }
    
        public virtual Material Material { get; set; }
    }
}