using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class PlanResultModel : BaseModel
    {
        public int statusDones;

        public string Id { get; set; }
        public string SBUName { get; set; }
        public string TaskId { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string TaskName { get; set; }
        public string ProductId { get; set; }
        public string ModuleId { get; set; }
        public string ProjectProductId { get; set; }
        public string ContractName { get; set; }
        public string ContractCode { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public decimal EstimateTime { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public int DataType { get; set; }
        public int DesignStatus { get; set; }
        public string ResponsiblePersion { get; set; }
        public string ResponsiblePersionName { get; set; }
        public decimal? ExecutionTime { get; set; }
        public DateTime? DesignFinishDate { get; set; }
        public DateTime? MakeFinishDate { get; set; }
        public DateTime? TransferDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? RealStartDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeId { get; set; }
        public string DesignCode { get; set; }
        public string DesignName { get; set; }
        public string ModuleGroupId { get; set; }
        public int Done { get; set; }
        public string ReferenceId { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public List<PlanResultModel> ListPlan { get; set; }
        public DateTime? ContractStartDue { get; set; }
        public DateTime? ContractDueDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public DateTime? PlanStartDue { get; set; }
        public int Types { get; set; }
        public int Duration { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public string SBUId { get; set; }
        public string WorkTypeId { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? EstimateTimes { get; set; }

        public PlanResultModel()
        {
            ListPlan = new List<PlanResultModel>();
        }
    }

    public class InforEmployee
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string SubId { get; set; }
        public string SubName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeCode { get; set; }

        public List<InforEmployee> ListInforEmployee { get; set; }
        public string UserId { get; set; }

        public InforEmployee()
        {
            ListInforEmployee = new List<InforEmployee>();
        }
    }


    public class WorkingTime
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string WorkType { get; set; }
        public List<WorkTimeModel> ListWorkingTime { get; set; }
        public WorkingTime()
        {
            ListWorkingTime = new List<WorkTimeModel>();
        }
    }


}
