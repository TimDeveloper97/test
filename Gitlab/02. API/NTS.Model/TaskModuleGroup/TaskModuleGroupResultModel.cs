using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskModuleGroup
{
    public class TaskModuleGroupResultModel
    {
        public string Id { get; set; }
        public int? Index { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string ModuleGroupId { get; set; }
        public string ModuleGroupName { get; set; }
    }
}
