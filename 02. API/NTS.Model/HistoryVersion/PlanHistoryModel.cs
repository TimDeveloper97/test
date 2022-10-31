using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class PlanHistoryModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectProductId { get; set; }
        public string StageId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> ContractStartDate { get; set; }
        public Nullable<System.DateTime> ContractDueDate { get; set; }
        public Nullable<System.DateTime> PlanStartDate { get; set; }
        public Nullable<System.DateTime> PlanDueDate { get; set; }
        public Nullable<System.DateTime> ActualStartDate { get; set; }
        public Nullable<System.DateTime> ActualEndDate { get; set; }
        public decimal EstimateTime { get; set; }
        public int DoneRatio { get; set; }
        public int Index { get; set; }
        public int Weight { get; set; }
        public string SupplierId { get; set; }
        public int TrackerType { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsPlan { get; set; }
    }
}
