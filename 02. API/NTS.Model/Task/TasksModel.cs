using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Task
{
    public class TasksModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }
        public decimal TimeStandard { get; set; }
        public string TaskModuleGroupId { get; set; }
    }
}
