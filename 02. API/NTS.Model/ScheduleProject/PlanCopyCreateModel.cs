using NTS.Model.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ScheduleProject
{
    public class PlanCopyCreateModel
    {
        public ScheduleProjectResultModel ScheduleProject { get; set; }
        public List<ScheduleEntity> ListCopy { get; set; }
        public List<ScheduleEntity> ListCopyStage { get; set; }
        public PlanCopyCreateModel()
        {
            ScheduleProject = new ScheduleProjectResultModel();
            ListCopy = new List<ScheduleEntity>();
            ListCopyStage = new List<ScheduleEntity>();
        }
    }
}
