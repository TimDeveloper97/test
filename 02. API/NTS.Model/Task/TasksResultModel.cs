using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Task
{
    public class TasksResultModel :BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TaskId { get; set; }
        public int Type { get; set; }
        public int Index { get; set; }
        public decimal TimeStandard { get; set; }
        public string Description { get; set; }
        public bool Checked { get; set; }
        public string TaskModuleGroupId { get; set; }
    }

    public class AvgModel
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public decimal Avg { get; set; }
    }
}
