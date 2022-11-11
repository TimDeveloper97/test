using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class PlanErrorFixByEmployeeModel
    {
        public string PlanId { get; set; }
        public string EmployeeId { get; set; }
        public decimal EstimateTime { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public int Type { get; set; }
    }
}
