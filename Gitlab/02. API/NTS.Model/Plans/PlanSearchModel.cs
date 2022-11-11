using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class PlanSearchModel : SearchCommonModel
    {
        public string DepartmentId { get; set; }
        public string TaskName { get; set; }
        public string ProjectId { get; set; }
        public string EmployeeCode { get; set; }
        public string DesignCode { get; set; }
        public string ContractCode { get; set; }
        public string EmployeeId { get; set; }
        public int Done { get; set; }
        public int DoneType { get; set; }
        public int Types { get; set; }
        public string Id { get; set; }
        public string SBUId { get; set; }
        public string WorkTypeId { get; set; }
        public int Status { get; set; }

        public DateTime? PlanStartDateFrom { get; set; }
        public DateTime? PlanStartDateTo { get; set; }

        public DateTime? PlanDueDateFrom { get; set; }
        public DateTime? PlanDueDateTo { get; set; }
    }
}
