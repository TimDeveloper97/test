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
    
    public partial class OutputRessultDocument
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string OutputRessultId { get; set; }
    
        public virtual OutputRessult OutputRessult { get; set; }
    }
}
