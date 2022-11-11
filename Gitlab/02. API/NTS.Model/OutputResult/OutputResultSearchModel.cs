using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.OutputResult
{
    public class OutputResultSearchModel: SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string FlowStageId { get; set; }
        public List<string> ListIdSelect { get; set; }
        public OutputResultSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
