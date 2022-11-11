using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ScheduleProject
{
    public class OTPlanModel
    {
        public int OT { get; set; }
        public List<ScheduleProjectResultModel> ListPlanOT { get; set; }
        public List<ScheduleProjectResultModel> ListPlan { get; set; }
        public OTPlanModel()
        {
            ListPlanOT = new List<ScheduleProjectResultModel>();
            ListPlan = new List<ScheduleProjectResultModel>();
        }
    }
}
