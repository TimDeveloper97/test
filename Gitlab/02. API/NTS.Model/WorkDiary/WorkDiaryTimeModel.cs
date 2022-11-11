using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkDiary
{
    public class WorkDiaryTimeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public string SBUId { get; set; }
        public string ProjectId { get; set; }
        public string ObjectId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string CreateBy { get; set; }
        public decimal TotalTime { get; set; }
        public DateTime? WorkDate { get; set; }

        public bool IsHoliday { get; set; }

        public decimal TotalWorkTime { get; set; }

        public List<DayOfMonthModel> ListWorkingTime { get; set; }

        public WorkDiaryTimeModel()
        {
            ListWorkingTime = new List<DayOfMonthModel>();
        }
    }

}
