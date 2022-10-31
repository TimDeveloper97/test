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
    
    public partial class NSMaterialParameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NSMaterialParameter()
        {
            this.NSMaterialParameterValues = new HashSet<NSMaterialParameterValue>();
        }
    
        public string Id { get; set; }
        public string NSMaterialGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string ConnectCharacter { get; set; }
        public string Unit { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual NSMaterialGroup NSMaterialGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NSMaterialParameterValue> NSMaterialParameterValues { get; set; }
    }
}
