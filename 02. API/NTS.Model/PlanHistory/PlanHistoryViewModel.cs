using NTS.Model.ScheduleProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PlanHistory
{
    public class PlanHistoryViewModel
    {
        public string ProjectName { get; set; }
        public int Version { get; set; }
        public DateTime? CreateDate { get; set; }
        public ResultModel Result { get; set; }
        public PlanHistoryViewModel()
        {
            Result = new ResultModel();
        }
    }
}
