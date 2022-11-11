using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Report
{
    public class ReportErrorWorkModel
    {
        public string Id { get; set; }
        public DateTime? PlanFinishDate { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? FinishDate { get; set; }
        public string StageId { get; set; }
        public int? AffectId { get; set; }
        public string ProjectId { get; set; }
        public int Status { get; set; }
        public int Deadline { get; set; }
        public string ProjectStatus { get; set; }
        public string FixByName { get; set; }
        public string EmployeeFixId { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SupportName { get; set; }
        public string ApproveName { get; set; }
        public string AdviseName { get; set; }
        public string NotifyName { get; set; }

        public string SupportId { get; set; }
        public string ApproveId { get; set; }
        public string AdviseId { get; set; }
        public string NotifyId { get; set; }

        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ErrorCode { get; set; }
        public string Subject { get; set; }
        public decimal? EstimateTime { get; set; }
        public string Solution { get; set; }
        public string CustomerId { get; set; }
        public string CustomerFinalId { get; set; }
        public string DepartmentManageId { get; set; }
        public string StageName { get; set; }
        public DateTime? ChangeDate { get; set; }
        public int TotalChange { get; set; }
    }
}
