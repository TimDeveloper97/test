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
    
    public partial class ProductModule
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    
        public virtual Product Product { get; set; }
    }
}