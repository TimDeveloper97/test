using NTS.Model.ErrorAttach;
using NTS.Model.ErrorHistory;
using NTS.Model.ErrorImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Error
{
    public class ErrorModel : BaseModel
    {
        public string Id { get; set; }
        public string ErrorGroupId { get; set; }
        public string ErrorGroupName { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ErrorGroupCode { get; set; }
        public string DepartmentProcessId { get; set; }
        public string DepartmentProcessName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Subject { get; set; }
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string ObjectCode { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorCode { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ErrorBy { get; set; }
        public string ErrorByName { get; set; }
        public string ErrorByCode { get; set; }
        public string QuickSolution { get; set; }
        public string Solution { get; set; }
        public int Status { get; set; }
        public int ObjectType { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public string FixBy { get; set; }
        public string FixByName { get; set; }
        public decimal ErrorCost { get; set; }
        public int? AffectId { get; set; }
        public Nullable<System.DateTime> PlanStartDate { get; set; }
        public string PlanStartDateView { get; set; }
        public Nullable<System.DateTime> PlanFinishDate { get; set; }
        public Nullable<System.DateTime> ActualStartDate { get; set; }
        public Nullable<System.DateTime> ActualFinishDate { get; set; }
        public Nullable<int> IsKCSConfirmed { get; set; }
        public Nullable<int> IsManagerConfirmed { get; set; }
        public Nullable<int> TempConfirmed { get; set; }
        public Nullable<System.DateTime> DesignCompleteTime { get; set; }
        public Nullable<System.DateTime> KCSCompleteTime { get; set; }
        public string Note { get; set; }
        public string PersonConfirmMail { get; set; }
        public string PersonProcessMail { get; set; }
        public string ModuleErrorReasonId { get; set; }
        public string ModuleErrorCostId { get; set; }
        public string ModuleErrorVisualId { get; set; }
        public string ModuleErrorVisualCode { get; set; }
        public string ModuleErrorVisualName { get; set; }
        public Nullable<int> IsAllowDownload { get; set; }
        public List<ErrorImageModel> ListImage { get; set; }
        //
        //public List<ErrorHistoryModel> ListHistory { get; set; }
        public bool FirstHistory { get; set; }
        //
        public List<ErrorAttachModel> ListFile { get; set; }
        public string strHistory { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public decimal Price { get; set; }
        public DateTime? KickOffDate { get; set; }
        public int Index { get; set; }
        public string AuthorDepartmentId { get; set; }
        public string AuthorDepartmentName { get; set; }

        public string Reason { get; set; }
        public bool isChange { get; set; }
        public bool CanChangeDate { get; set; }

        public List<ErrorFixModel> Fixs { get; set; }
        public string ErrorId { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Done { get; set; }
        public DateTime? DateFrom { get; set; }
        public string Statuss { get; set; }
        public string EmployeeFixId { get; set; }
        public string FinishDates { get; set; }

        public ErrorModel()
        {
            Fixs = new List<ErrorFixModel>();
        }
    }
}
