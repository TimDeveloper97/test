using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskTimeStandardModel
{
    public class CalculateAverageTaskTimeStandardResultModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string ModuleGroupId { get; set; }
        public string ModuleGroupCode { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public decimal Average { get; set; }
    }
}
