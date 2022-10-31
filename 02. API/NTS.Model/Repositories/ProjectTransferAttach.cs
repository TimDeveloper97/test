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
    
    public partial class ProjectTransferAttach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectTransferAttach()
        {
            this.ProjectTransferProducts = new HashSet<ProjectTransferProduct>();
        }
    
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string NumberOfReport { get; set; }
        public Nullable<System.DateTime> SignDate { get; set; }
        public string FileName { get; set; }
        public Nullable<decimal> FileSize { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectTransferProduct> ProjectTransferProducts { get; set; }
    }
}
