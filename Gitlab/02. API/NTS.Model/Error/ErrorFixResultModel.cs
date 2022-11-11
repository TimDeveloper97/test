using NTS.Model.ErrorAttach;
using NTS.Model.ErrorImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Error
{
    public class ErrorFixResultModel
    {
        public string Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentId { get; set; }
        public string FixByName { get; set; }
        public string Solution { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public decimal? EstimateTime { get; set; }
        public int Status { get; set; }
        public string SupportName { get; set; }
        public string ApproveName { get; set; }
        public string AdviseName { get; set; }
        public string NotifyName { get; set; }
        public int Deadline { get; set; }

        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ErrorId { get; set; }
        public string ErrorCode { get; set; }
        public string Subject { get; set; }
        public int Type { get; set; }
        public int CountChangePlan { get; set; }

        public Nullable<System.DateTime> PlanStartDate { get; set; }
        public Nullable<System.DateTime> PlanFinishDate { get; set; }
        public string PlanStartDateView { get; set; }
        public int Done { get; set; }
        public string ErrorByName { get; set; }

        public ErrorFixResultModel()
        {
        }
    }
}
