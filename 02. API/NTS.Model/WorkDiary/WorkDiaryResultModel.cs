using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkDiary
{
    public class WorkDiaryResultModel
    {
        public string Id { get; set; }
        public string WorkTimeId { get; set; }
        public string ModuleId { get; set; }
        public string ProjectId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public Nullable<System.DateTime> WorkDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal TotalTime { get; set; }
        public int PercentFinish { get; set; }
        public string Address { get; set; }
        public string Content { get; set; }
    }
}
