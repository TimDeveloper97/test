using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class ScheduleUpdateModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string StageId { get; set; }
        public string Description { get; set; }
        public string ProjectProductId { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public decimal EsimateTime { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int Weight { get; set; }
        public string SupplierId { get; set; }
    }
}
