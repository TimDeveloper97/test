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
    
    public partial class StepInQuotation
    {
        public string Id { get; set; }
        public string QuotationStepId { get; set; }
        public string QuotationId { get; set; }
        public int Index { get; set; }
        public int Rate { get; set; }
        public int Status { get; set; }
    }
}