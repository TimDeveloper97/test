using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskFlowStage
{
    public class TaskFlowStageSearchModel: SearchCommonModel
    {
        public string FlowStageId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsDesignModule { get; set; }
        public int? Type { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string CodeInput { get; set; }
        public string CodeOutput { get; set; }

        public List<string> ListIdSelect { get; set; }
        public bool? IsProjectWork { get; set; }

        public TaskFlowStageSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
