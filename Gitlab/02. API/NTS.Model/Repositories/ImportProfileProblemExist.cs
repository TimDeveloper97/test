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
    
    public partial class ImportProfileProblemExist
    {
        public string Id { get; set; }
        public string ImportProfileId { get; set; }
        public string Note { get; set; }
        public string Plan { get; set; }
        public int Step { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}