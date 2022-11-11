using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Module
{
    public class StageModel
    {
        public string Id { get; set; }
        public string StageId { get; set; }
        public string Name { get; set; }
        public string ModuleGroupId { get; set; }
        public decimal Time { get; set; }
    }
}
