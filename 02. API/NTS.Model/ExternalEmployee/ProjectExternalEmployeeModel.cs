using NTS.Model.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExternalEmployee
{
    public class ProjectExternalEmployeeModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }   
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string TaxCode { get; set; }
        public string IdentifyNum { get; set; }
        public string IdentifyAddress { get; set; }
        public DateTime IdentifyDate { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentAddressProvinceId { get; set; }
        public string CurrentAddressDistrictId { get; set; }
        public string CurrentAddressWardId { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string EmployeeId { get; set; }
        public int Type { get; set; }
        public string ProjectId { get; set; }
        public string RoleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string JobDescription { get; set; }
        public int Evaluate { get; set; }
        public bool Status { get; set; }
        public decimal Subsidy { get; set; }
        public DateTime? SubsidyStartTime { get; set; }
        public DateTime? SubsidyEndTime { get; set; }
    }
}
