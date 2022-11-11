using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskModuleGroup
{
    public class TaskModuleGroupSearchModel : SearchCommonModel
    {
        public string ModuleGroupId { get; set; }
        public string TaskId { get; set; }
    }
}
