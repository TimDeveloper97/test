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
    
    public partial class QCResult
    {
        public string Id { get; set; }
        public string QCCheckListId { get; set; }
        public string ProductItemId { get; set; }
        public int Status { get; set; }
        public System.DateTime QCDate { get; set; }
        public string QCBy { get; set; }
        public string Reason { get; set; }
    }
}
