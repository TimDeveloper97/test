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
    
    public partial class SubjectSkill
    {
        public string Id { get; set; }
        public string SubjectId { get; set; }
        public string SkillId { get; set; }
    
        public virtual Skill Skill { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
