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
    
    public partial class ActivityLog
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public System.DateTime LogDate { get; set; }
        public string LogContent { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string TableName { get; set; }
        public string ObjectId { get; set; }
        public int LogType { get; set; }
    
        public virtual User User { get; set; }
    }
}
