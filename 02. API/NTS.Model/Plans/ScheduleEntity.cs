using NTS.Model.ScheduleProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class ScheduleEntity
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public int Duration { get; set; }
        public int DoneRatio { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public string PlanName { get; set; }
        public string ParentId { get; set; }
        public int DataType { get; set; }
        public string ResponsiblePersionName { get; set; }
        public decimal? EstimateTime { get; set; }
        public string ProjectProductId { get; set; }
        public decimal? RealQuantity { get; set; }
        public string NameView { get; set; }
        public int Status { get; set; }
        public string ContractIndex { get; set; }
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
        public string ProjectId { get; set; }
        public string MainUserId { get; set; }

        public int ModuleStatus { get; set; }
        public int LoadIndex { get; set; }
        public List<string> ListIdUserId = new List<string>();
        public List<GrantChartModel> ListGrantChart = new List<GrantChartModel>();

    }
}
