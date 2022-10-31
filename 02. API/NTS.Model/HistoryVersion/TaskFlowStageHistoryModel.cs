using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class TaskFlowStageHistoryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string Code { get; set; }
        public Nullable<bool> IsDesignModule { get; set; }
        public Nullable<decimal> TimeStandard { get; set; }
        public string DegreeId { get; set; }
        public string Specialization { get; set; }
        public string SpecializeId { get; set; }
        public string WorkTypeRId { get; set; }
        public string WorkTypeAId { get; set; }
        public string WorkTypeSId { get; set; }
        public string WorkTypeCId { get; set; }
        public string WorkTypeIId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string FlowStageId { get; set; }
        public Nullable<decimal> PercentValue { get; set; }
    }
}
