using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class PlanErrorFixResultModel
    {
        public string Id { get; set; }
        public string ResponsiblePersion { get; set; }
        public string ResponsiblePersionId { get; set; }
        public string EmployeeName { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public decimal? EstimateTime { get; set; }
        public decimal Quantity { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public string DesignCode { get; set; }
        public string DesignName { get; set; }
        public string IndustryCode { get; set; }
        public decimal PricingModule { get; set; }
        public string DataType { get; set; }
        public string ModuleStatusView { get; set; }
        public string DesignStatusView { get; set; }
        public DateTime? ExpectedDesignFinishDate { get; set; }
        public DateTime? ExpectedMakeFinishDate { get; set; }
        public DateTime? ExpectedTransferDate { get; set; }
        public DateTime? KickOffDate { get; set; }
        public string PlanId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectProductId { get; set; }
        public string EpCode { get; set; }
        public string ObjectName { get; set; }
    }
}
