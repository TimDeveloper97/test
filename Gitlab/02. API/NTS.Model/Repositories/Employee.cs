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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.CoefficientEmployees = new HashSet<CoefficientEmployee>();
            this.EmployeeBankAccounts = new HashSet<EmployeeBankAccount>();
            this.Users = new HashSet<User>();
            this.EmployeeChangeIncomes = new HashSet<EmployeeChangeIncome>();
            this.EmployeeChangeInsurances = new HashSet<EmployeeChangeInsurance>();
            this.EmployeeCourseTrainings = new HashSet<EmployeeCourseTraining>();
            this.EmployeeDegrees = new HashSet<EmployeeDegree>();
            this.EmployeeEducations = new HashSet<EmployeeEducation>();
            this.EmployeeHistoryAppoints = new HashSet<EmployeeHistoryAppoint>();
            this.EmployeeHistoryJobTranfers = new HashSet<EmployeeHistoryJobTranfer>();
            this.EmployeeSkills = new HashSet<EmployeeSkill>();
            this.EmployeeSkillDetails = new HashSet<EmployeeSkillDetail>();
            this.EmployeeWorkHistories = new HashSet<EmployeeWorkHistory>();
            this.Families = new HashSet<Family>();
        }
    
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        public string QualificationId { get; set; }
        public string JobPositionId { get; set; }
        public string EmployeeGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public string BankAccount { get; set; }
        public string SocialInsurrance { get; set; }
        public string HealtInsurrance { get; set; }
        public string TaxCode { get; set; }
        public string IdentifyNum { get; set; }
        public Nullable<System.DateTime> StartWorking { get; set; }
        public Nullable<System.DateTime> EndWorking { get; set; }
        public string Carrer { get; set; }
        public int Status { get; set; }
        public string WorkTypeId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public decimal Index { get; set; }
        public Nullable<int> IsOfficial { get; set; }
        public string PersonalEmail { get; set; }
        public Nullable<System.DateTime> IdentifyDate { get; set; }
        public string IdentifyAddress { get; set; }
        public string AddressProvinceId { get; set; }
        public string AddressDistrictId { get; set; }
        public string AddressWardId { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentAddressProvinceId { get; set; }
        public string PermanentAddressDistrictId { get; set; }
        public string PermanentAddressWardId { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentAddressProvinceId { get; set; }
        public string CurrentAddressDistrictId { get; set; }
        public string CurrentAddressWardId { get; set; }
        public Nullable<System.DateTime> StartInsurance { get; set; }
        public string BookNumberInsurance { get; set; }
        public string CardNumberInsurance { get; set; }
        public string Kcb { get; set; }
        public string InsuranceLevelId { get; set; }
        public string Forte { get; set; }
        public int MaritalStatus { get; set; }
        public string Seniority { get; set; }
        public string ReasonEndWorkingId { get; set; }
        public string WorkLocationId { get; set; }
        public string AppliedPositionId { get; set; }
        public Nullable<System.DateTime> ContractExpirationDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoefficientEmployee> CoefficientEmployees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeBankAccount> EmployeeBankAccounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeChangeIncome> EmployeeChangeIncomes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeChangeInsurance> EmployeeChangeInsurances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeCourseTraining> EmployeeCourseTrainings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeDegree> EmployeeDegrees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeHistoryAppoint> EmployeeHistoryAppoints { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeHistoryJobTranfer> EmployeeHistoryJobTranfers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeSkillDetail> EmployeeSkillDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeWorkHistory> EmployeeWorkHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Family> Families { get; set; }
    }
}
