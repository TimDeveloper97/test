using NTS.Model.ScheduleProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class PlanAdjustmentCreateModel
    {
        public string PlanId { get; set; }
        public List<EmployeePlanModel> ListPlanAdjustment { get; set; }
        public PlanAdjustmentCreateModel()
        {
            ListPlanAdjustment = new List<EmployeePlanModel>();
        }
    }
}
