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
    
    public partial class WorkTypeInterviewQuestion
    {
        public int Id { get; set; }
        public int WorkTypeInterviewId { get; set; }
        public string QuestionId { get; set; }
    
        public virtual WorkTypeInterview WorkTypeInterview { get; set; }
    }
}
