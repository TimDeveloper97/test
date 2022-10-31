using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class WorkTimeModel
    {
        public DateTime DateTime { set; get; }
        public decimal EstimateTime { get; set; }
        public bool IsHoliday { get; set; }
        public List<string> PlanId { get; set; }
        public List<string> ErrorFixId { get; set; }

        public List<PlanErrorFixByEmployeeModel> Plans = new List<PlanErrorFixByEmployeeModel>();
    }

    public class PlanWorkTimeModel
    {
        public string EmployeeId { set; get; }
        public string PlanId { set; get; }
        public string ErrorFixId { set; get; }
        public DateTime DateTime { set; get; }
        public decimal EstimateTime { get; set; }
        public bool IsHoliday { get; set; }
    }
}
