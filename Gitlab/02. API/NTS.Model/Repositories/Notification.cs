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
    
    public partial class Notification
    {
        public long NotificationId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string Image { get; set; }
        public string ObjectId { get; set; }
        public Nullable<int> ObjectType { get; set; }
        public string Link { get; set; }
        public string Status { get; set; }
    
        public virtual User User { get; set; }
    }
}
