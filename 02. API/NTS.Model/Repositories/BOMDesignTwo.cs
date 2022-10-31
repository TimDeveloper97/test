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
    
    public partial class BOMDesignTwo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BOMDesignTwo()
        {
            this.BOMDesignTwoDetials = new HashSet<BOMDesignTwoDetial>();
            this.BOMDesignTwoAttaches = new HashSet<BOMDesignTwoAttach>();
        }
    
        public string Id { get; set; }
        public string ProjectProductId { get; set; }
        public int Version { get; set; }
        public int Index { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BOMDesignTwoDetial> BOMDesignTwoDetials { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BOMDesignTwoAttach> BOMDesignTwoAttaches { get; set; }
        public virtual ProjectProduct ProjectProduct { get; set; }
    }
}
