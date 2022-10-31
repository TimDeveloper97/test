using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkDiary
{
    public class ProjectWorkTimeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public decimal TotalWorkTime { get; set; }
        public List<WorkDiaryTimeModel> ListWorkDiaryTime { get; set; }
        public List<WorkDiaryTimeModel> ListWorkDiaryTimeNoProjectId { get; set; }
        public List<decimal> ListMondayWorkDiaryTime { get; set; }

        public ProjectWorkTimeModel()
        {
            ListWorkDiaryTime= new List<WorkDiaryTimeModel>();
            ListWorkDiaryTimeNoProjectId = new List<WorkDiaryTimeModel>();
            ListMondayWorkDiaryTime = new List<decimal>();
        }
    }
}
