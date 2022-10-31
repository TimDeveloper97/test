using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class PlanModel : BaseModel
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectProductId { get; set; }
        public string StageId { get; set; }
        public string ParentId { get; set; }
        public int DataType { get; set; }
        public string ModuleId { get; set; }
        public string ProductId { get; set; }
        public string ResponsiblePersion { get; set; }
        public decimal ExecutionTime { get; set; }
        public decimal TotalTime { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public int Status { get; set; }
        public string EmployeeId { get; set; }
        public decimal EsimateTime { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int OT { get; set; }
        public string ReferenceId { get; set; }
        public string OriginalProject { get; set; }
        public string Description { get; set; }
        public int Types { get; set; }
        public int Done { get; set; }
        public int StatusDone { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractDueDate { get; set; }
        public int DoneRatio { get; set; }
        public int Weight { get; set; }
        public string SupplierId { get; set; }
        public int TrackerType { get; set; }
        public bool IsPlan { get; set; }
        public bool IsScheduleProject { get; set; }
    }

    public class PlanViewModel 
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectProductId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }
        public decimal EstimateTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? RealStartDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public string DepartmentName { get; set; }
        public string SBUName { get; set; }
        public int Status { get; set; }
        public decimal EsimateTime { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int OT { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? MakeFinishDate { get; set; }
        public DateTime? DesignFinishDate { get; set; }
        public string DesignCode { get; set; }
        public string ContractName { get; set; }
        public string Description { get; set; }
    }
}
