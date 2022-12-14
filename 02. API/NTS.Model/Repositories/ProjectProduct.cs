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
    
    public partial class ProjectProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectProduct()
        {
            this.BOMDesignTwoes = new HashSet<BOMDesignTwo>();
            this.IncurredMaterials = new HashSet<IncurredMaterial>();
            this.ModuleMaterialFinishDesigns = new HashSet<ModuleMaterialFinishDesign>();
        }
    
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ParentId { get; set; }
        public string ModuleId { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public string Specifications { get; set; }
        public int DataType { get; set; }
        public int ModuleStatus { get; set; }
        public int DesignStatus { get; set; }
        public Nullable<System.DateTime> DesignFinishDate { get; set; }
        public Nullable<System.DateTime> MakeFinishDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public string Note { get; set; }
        public string ProductId { get; set; }
        public Nullable<System.DateTime> ExpectedDesignFinishDate { get; set; }
        public Nullable<System.DateTime> ExpectedMakeFinishDate { get; set; }
        public Nullable<System.DateTime> ExpectedTransferDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal RealQuantity { get; set; }
        public decimal Price { get; set; }
        public string ContractIndex { get; set; }
        public bool IsGeneralDesign { get; set; }
        public bool DesignWorkStatus { get; set; }
        public Nullable<System.DateTime> DesignCloseDate { get; set; }
        public Nullable<System.DateTime> GeneralDesignLastDate { get; set; }
        public bool MaterialExist { get; set; }
        public bool IsIncurred { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int Weight { get; set; }
        public int Duration { get; set; }
        public int DoneRatio { get; set; }
        public Nullable<System.DateTime> ContractStartDate { get; set; }
        public Nullable<System.DateTime> ContractDueDate { get; set; }
        public Nullable<System.DateTime> PlanStartDate { get; set; }
        public Nullable<System.DateTime> PlanDueDate { get; set; }
        public int Status { get; set; }
        public bool IsCatalogRequire { get; set; }
        public bool IsUserGuideRequire { get; set; }
        public bool IsMaintenaineGuideRequire { get; set; }
        public bool IsPracticeGuideRequire { get; set; }
        public bool IsNeedQC { get; set; }
        public int QCQuantity { get; set; }
        public string CatalogRequireNote { get; set; }
        public string UserGuideRequireNote { get; set; }
        public string MaintenaineGuideRequireNote { get; set; }
        public string PracticeGuideRequireNote { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BOMDesignTwo> BOMDesignTwoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncurredMaterial> IncurredMaterials { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModuleMaterialFinishDesign> ModuleMaterialFinishDesigns { get; set; }
    }
}
