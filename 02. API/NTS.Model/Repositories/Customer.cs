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
    
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.CustomerContacts = new HashSet<CustomerContact>();
            this.CustomerOfCustomers = new HashSet<CustomerOfCustomer>();
            this.CustomerRequirements = new HashSet<CustomerRequirement>();
        }
    
        public string Id { get; set; }
        public string CustomerTypeId { get; set; }
        public string Name { get; set; }
        public string CodeChar { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Contact { get; set; }
        public string Note { get; set; }
        public string SBUId { get; set; }
        public decimal Acreage { get; set; }
        public int EmployeeQuantity { get; set; }
        public decimal Capital { get; set; }
        public string Field { get; set; }
        public int Index { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string EmployeeId { get; set; }
        public string ProvinceId { get; set; }
        public string TaxCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerContact> CustomerContacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerOfCustomer> CustomerOfCustomers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerRequirement> CustomerRequirements { get; set; }
    }
}