using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GanttChart
{
    public class GanttChartModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string ProjectProductId { get; set; }
        public string Name { get; set; }
        public int DoneRatio { get; set; }
        public string Color { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public string ContractIndex { get; set; }
        public int DataType { get; set; }
        public int Index { get; set; }
        public int IndexPlan { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public string StageId { get; set; }
        public bool IsPlan { get; set; }
        public string SupplierId { get; set; }
        public int Status { get; set; }
        public int LoadIndex { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractDueDate { get; set; }
        public int InternalStatus { get; set; }
        public List<GanttChartModel> ListChild { get; set; }
        public GanttChartModel()
        {
            ListChild = new List<GanttChartModel>();
        }
    }
}
