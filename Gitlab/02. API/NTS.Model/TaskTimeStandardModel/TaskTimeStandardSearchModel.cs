using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskTimeStandardModel
{
    public class TaskTimeStandardSearchModel : SearchCommonModel
    {
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public string ModuleGroupId { get; set; }
        public string Name { get; set; }
        public string SBUId { get; set; }
        public int Type { get; set; }
        public string TaskId { get; set; }
    }
}
