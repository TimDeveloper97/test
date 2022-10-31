using NTS.Model.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskTimeStandardModel
{
    public class TaskTimeStandardResultModel
    {
        public string Id { get; set; }
        public string SBUName { get; set; }
        public string SBUId { get; set; }
        public string DepartmantName { get; set; }
        public string DepartmantId { get; set; }
        public string ModuleGroupId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string WorkTypeName { get; set; }
        public int TimeStandard { get; set; }
        public string TaskId { get; set; }
        public int Index { get; set; }
        public List<TaskStandardTypeModel> ListWorkType { get; set; }
        public List<TaskTimeStandardResultModel> ListData{ get; set; }
        public List<int> ListType { get; set; }
        public  DateTime UpdateDate { get; set; }
    }
}
