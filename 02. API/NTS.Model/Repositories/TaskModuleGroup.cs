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
    
    public partial class TaskModuleGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskModuleGroup()
        {
            this.TaskModuleGroupTimeStandards = new HashSet<TaskModuleGroupTimeStandard>();
        }
    
        public string Id { get; set; }
        public string ModuleGroupId { get; set; }
        public string TaskId { get; set; }
        public int Index { get; set; }
    
        public virtual ModuleGroup ModuleGroup { get; set; }
        public virtual Task Task { get; set; }
        public virtual Task Task1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskModuleGroupTimeStandard> TaskModuleGroupTimeStandards { get; set; }
    }
}
