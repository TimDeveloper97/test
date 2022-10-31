using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskFlowStage
{
    public class TaskFlowStageSearchResultModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDesignModule { get; set; }
        public string FlowStageId { get; set; }
        public string FlowStageName { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Type { get; set; }
        public string CodeInput { get; set; }
        public string CodeOutput { get; set; }

        public List<string> TaskInputResult { get; set; }
        public List<string> TaskOutPutResult { get; set; }

        public DateTime CreateDate { get; set; }
        public bool IsProjectWork { get; set; }
        public string WorkTypeR { get; set; }
        public string WorkTypeA { get; set; }
        public string WorkTypeS { get; set; }
        public string WorkTypeC { get; set; }
        public string WorkTypeI { get; set; }
        public List<string> Departments = new List<string>();
        public List<RASCIEntity> RASCI = new List<RASCIEntity>();
    }
}
