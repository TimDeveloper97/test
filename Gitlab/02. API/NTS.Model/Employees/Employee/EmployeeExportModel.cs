using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employees.Employee
{
    public class EmployeeExportModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string EmailCompany { get; set; }
        public string Email { get; set; }
        public string IdentifyNum { get; set; }
        public DateTime? IdentifyDate { get; set; }
        public string IdentifyAddress { get; set; }
        public int Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public int MaritalStatus { get; set; }
        public string Address { get; set; }
        public string AddressProvinceName { get; set; }
        public string AddressDistrictName { get; set; }
        public string AddressWardName { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentAddressProvinceName { get; set; }
        public string PermanentAddressDistrictName { get; set; }
        public string PermanentAddressWardName { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentAddressProvinceName { get; set; }
        public string CurrentAddressDistrictName { get; set; }
        public string CurrentAddressWardName { get; set; }
        public string TaxCode { get; set; }
        public DateTime? StartInsurance { get; set; }
        public string BookNumberInsurance { get; set; }
        public string CardNumberInsurance { get; set; }
        public string Kcb { get; set; }
        public string InsuranceLevelName { get; set; }
        public string InsuranceMoney { get; set; }
        public string Forte { get; set; }
        public string SBUName { get; set; }
        public string SBUId { get; set; }
        public string SBUCode { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public DateTime? StartWorking { get; set; }
        public string Seniority { get; set; }
        public int? IsOfficial { get; set; }
        public string IsOfficialName { get; set; }
        public string JobPositionId { get; set; }
        public string JobPositionName { get; set; }
        public string JobPositionCode { get; set; }
        public string WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public string WorkTypeCode { get; set; }
        public string WorkLocationName { get; set; }
        public string AppliedPositionName { get; set; }
        public string EmployeeGroupName { get; set; }
        public string StatusName { get; set; }
        public int Status { get; set; }
        public string UserNameId { get; set; }
        public string UserName { get; set; }
    }
}
