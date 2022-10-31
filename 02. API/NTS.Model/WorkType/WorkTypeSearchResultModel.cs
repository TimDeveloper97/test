using NTS.Model.Document;
using NTS.Model.Materials;
using NTS.Model.TaskFlowStage;
using NTS.Model.WorkTypeSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkType
{
    public class WorkTypeSearchResultModel
    {
        public string Id { get; set; }
        public decimal Quantity { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string FlowStageId { get; set; }
        public string FlowStageName { get; set; }

        public WorkTypeSearchResultModel()
        {
        }
    }
}
