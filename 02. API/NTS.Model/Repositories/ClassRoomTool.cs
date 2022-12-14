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
    
    public partial class ClassRoomTool
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClassRoomTool()
        {
            this.ClassRoomToolPractices = new HashSet<ClassRoomToolPractice>();
            this.ClassRoomToolProducts = new HashSet<ClassRoomToolProduct>();
            this.ClassRoomToolSkills = new HashSet<ClassRoomToolSkill>();
        }
    
        public string Id { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassRoomToolPractice> ClassRoomToolPractices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassRoomToolProduct> ClassRoomToolProducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassRoomToolSkill> ClassRoomToolSkills { get; set; }
    }
}
