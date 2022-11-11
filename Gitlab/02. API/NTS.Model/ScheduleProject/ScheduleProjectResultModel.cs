using NTS.Model.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ScheduleProject
{
    public class ScheduleProjectResultModel
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public string EmployeeName { get; set; }
        public string PlanName { get; set; }
        public string PlanCode { get; set; }
        public string ProjectId { get; set; }
        public string ParentId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleGroupId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public int DataType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? RealStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string PlanId { get; set; }
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }
        public string SbuId { get; set; }
        public string ResponsiblePersion { get; set; }
        public string ResponsiblePersionName { get; set; }
        public string EmployeeCode { get; set; }
        public string DatatypeName { get; set; }
        public decimal ExecutionTime { get; set; }

        public decimal? EstimateTime { get; set; }

        public string StartDateView { get; set; }
        public string RealStartDateView { get; set; }
        public string EndDateView { get; set; }
        public string RealEndDateView { get; set; }
        public string ProjectProductId { get; set; }
        public string PracticeName { get; set; }
        public string PracticeCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal? RealQuantity { get; set; }
        public string ModuleStatusView { get; set; }
        public string DesignStatusView { get; set; }
        public DateTime? KickOffDate { get; set; }
        public string KickOffDateView { get; set; }
        public DateTime? DesignFinishDate { get; set; }
        public string DesignFinishDateView { get; set; }
        public DateTime? MakeFinishDate { get; set; }
        public string MakeFinishDateView { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryDateView { get; set; }
        public DateTime? TransferDate { get; set; }
        public string TransferDateView { get; set; }
        public DateTime? ExpectedDesignFinishDate { get; set; }
        public string ExpectedDesignFinishDateView { get; set; }
        public DateTime? ExpectedMakeFinishDate { get; set; }
        public string ExpectedMakeFinishDateView { get; set; }
        public DateTime? ExpectedTransferDate { get; set; }
        public string ExpectedTransferDateView { get; set; }
        public string NameView { get; set; }
        public decimal PricingModule { get; set; }
        public string StatusView { get; set; }
        public int Status { get; set; }
        public string IndustryCode { get; set; }
        public string DesignName { get; set; }
        public string DesignCode { get; set; }
        public string StatusProject { get; set; }
        public int StatusProjectInt { get; set; }
        public int TaskTimeStandard { get; set; }
        public string TaskId { get; set; }
        public string CreateByName { get; set; }
        public string ReferenceId { get; set; }
        public string ContractIndex { get; set; }
        public decimal DoneRatio { get; set; }
        public decimal Done { get; set; }
        public int OT { get; set; }
        public string Description { get; set; }
        public string BackgroundColor { get; set; }
        public string StageName { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractDueDate { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public string StageId { get; set; }
        public string Color { get; set; }
        public int Weight { get; set; }
        public bool IsPlan { get; set; }
        public string InternalStatus { get; set; }
        public int? WorkTime { get; set; }
        public int Colspan { get; set; }
        public string SupplierId { get; set; }
        public int Type { get; set; }
        public DateTime CreateDate { get; set; }
        public int IndexPlan { get; set; }
        public List<string> ListIdUserId { get; set; }
        public List<GrantChartModel> ListGrantChart { get; set; }

        public ScheduleProjectResultModel()
        {
            Weight = 1;
            Colspan = 1;
            ListIdUserId = new List<string>();
            ListGrantChart = new List<GrantChartModel>();
        }
    }

    public class ResultModel
    {
        public List<DateTimeModel> holidays { get; set; }
        public List<DateTime> daysOfMonth { get; set; }
        public List<int> dayOfWeek { get; set; }
        public List<ScheduleEntity> listResult { get; set; }

        public ResultModel()
        {
            holidays = new List<DateTimeModel>();
            daysOfMonth = new List<DateTime>();
            dayOfWeek = new List<int>();
            listResult = new List<ScheduleEntity>();

        }
    }
}
