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
    
    public partial class ProductStandard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductStandard()
        {
            this.ModuleGroupProductStandards = new HashSet<ModuleGroupProductStandard>();
            this.ModuleProductStandards = new HashSet<ModuleProductStandard>();
            this.ProductStandardAttaches = new HashSet<ProductStandardAttach>();
            this.ProductStandardHistories = new HashSet<ProductStandardHistory>();
        }
    
        public string Id { get; set; }
        public string ProductStandardGroupId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Target { get; set; }
        public string Note { get; set; }
        public string Version { get; set; }
        public string EditContent { get; set; }
        public int DataType { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string OK_Images { get; set; }
        public string NG_Images { get; set; }
        public string ParentId { get; set; }
    
        public virtual Department Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModuleGroupProductStandard> ModuleGroupProductStandards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModuleProductStandard> ModuleProductStandards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductStandardAttach> ProductStandardAttaches { get; set; }
        public virtual ProductStandardGroup ProductStandardGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductStandardHistory> ProductStandardHistories { get; set; }
        public virtual SBU SBU { get; set; }
    }
}
