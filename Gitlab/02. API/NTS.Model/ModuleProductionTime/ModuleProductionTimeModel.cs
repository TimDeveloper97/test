using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleProductionTime
{
    public class ModuleProductionTimeModel: BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string StageId { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }
        public int ExecutionTime { get; set; }
    }
}
