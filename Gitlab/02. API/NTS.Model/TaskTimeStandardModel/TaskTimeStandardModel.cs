using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskTimeStandardModel
{
    public class TaskTimeStandardModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string TaskId { get; set; }
        public string EmployeeName { get; set; }
        public string SBUId { get; set; }
        public string DepartmantId { get; set; }
        public decimal TimeStandard { get; set; }
        public int Type { get; set; }
        public int Index { get; set; }

        public List<TaskTimeStandardModel> ListWorkType { get; set; }
        public DateTime UpdateDate { get; set; }
    }
       
    public class ReportErrorModel
    {
        public string Name { get; set; }
        public List<int> ListCheck { get; set; }
        public ReportErrorModel()
        {
            ListCheck = new List<int>();
        }
    }
}
