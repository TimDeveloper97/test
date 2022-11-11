using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class PlanInfoModel
    {
        public string Id { get; set; }
        public string ProjectName { get; set; }
        public string ResponsiblePersion { get; set; }
        public string Name { get; set; }
        public string PersonInCharge { get; set; }
        public string Description { get; set; }
        public int DoneRatio { get; set; }
        public int Type { get; set; }
        public int Index { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
    }
}
