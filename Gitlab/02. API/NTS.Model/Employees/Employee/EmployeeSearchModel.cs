using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employees
{
    public class EmployeeSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameCode { get; set; }
        public string CourseId { get; set; }
        public string JobPositionId { get; set; }
        public string PhoneNumber { get; set; }
        public int? Gender { get; set; }
        public int? IsReach { get; set; }
        public bool IsExport { get; set; }
        public List<string> ListIdSelect { get; set; }
        public string UserName { get; set; }
        public string SBUName { get; set; }
        public string SBUId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentId { get; set; }
        public int? Status { get; set; }
        public DateTime? StartWorkingFrom { get; set; }
        public DateTime? StartWorkingTo { get; set; }
        public DateTime? ContractExpirationDateFrom { get; set; }
        public DateTime? ContractExpirationDateTo { get; set; }
        public string TaxCode { get; set; }
        public string WorkTypeId { get; set; }
    }
}
