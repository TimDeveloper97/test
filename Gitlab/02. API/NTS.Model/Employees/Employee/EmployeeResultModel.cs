using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employees
{
    public class EmployeeResultModel
    {
        public string Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentId { get; set; }
        public string WorkTypeId { get; set; }
        public string QualificationName { get; set; }
        public string QualificationID { get; set; }
        public string JobPositionName { get; set; }
        public string JobPositionId { get; set; }
        public string Code { get; set; }
        public string CourseId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; }
        public int IsDisable { get; set; }
        public int IsReach { get; set; }
        public decimal Grade { get; set; }
        public decimal Mark { get; set; }
        public string IdentifyNum { get; set; }
        public string UserName { get; set; }
        public string UserNameId { get; set; }
        public DateTime? StartWorking { get; set; }
        public DateTime? EndWorking { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string WorkType { get; set; }
        public string WorkTypeName { get; set; }
        public string TaxCode { get; set; }
        public int CourseNumber { get; set; }
        public DateTime? ContractExpirationDate { get; set; }
        public int TotalCourse { get; set; }

    }
}
