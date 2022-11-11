using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.FlowStage
{
    public class FlowStageSearchModel: SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public List<string> ListIdSelect { get; set; }
        public FlowStageSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
