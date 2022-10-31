using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
  public  class PlanEndDateModel
    {
        public DateTime StartDate { get; set; }
        public decimal EstimateTime { get; set; }
        public int OT { get; set; }
    }
}
