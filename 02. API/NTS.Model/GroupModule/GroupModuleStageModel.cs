using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GroupModule
{
    public class GroupModuleStageModel
    {
        public string Id { get; set; }
        public bool IsDelete { get; set; }

        public string ModuleGroupId { get; set; }
        public string ModuleGroupName { get; set; }
        public string StageId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public string SBUName { get; set; }
        public string DepartmentName { get; set; }
        public decimal Time { get; set; }
    }
}
