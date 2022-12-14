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
    
    public partial class DataDistribution
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DataDistribution()
        {
            this.DataDistributionFileLinks = new HashSet<DataDistributionFileLink>();
        }
    
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string Description { get; set; }
        public Nullable<int> Type { get; set; }
        public string Path { get; set; }
        public bool IsExportMaterial { get; set; }
    
        public virtual Department Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DataDistributionFileLink> DataDistributionFileLinks { get; set; }
    }
}
