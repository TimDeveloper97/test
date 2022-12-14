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
    
    public partial class ProjectAttach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectAttach()
        {
            this.ProjectAttachUsers = new HashSet<ProjectAttachUser>();
        }
    
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> FileSize { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
        public int Index { get; set; }
        public int Type { get; set; }
        public int PromulgateType { get; set; }
        public string CustomerId { get; set; }
        public string SupplierId { get; set; }
        public Nullable<System.DateTime> PromulgateDate { get; set; }
        public string GroupName { get; set; }
        public bool IsRequired { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string PDFLinkFile { get; set; }
    
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectAttachUser> ProjectAttachUsers { get; set; }
    }
}
