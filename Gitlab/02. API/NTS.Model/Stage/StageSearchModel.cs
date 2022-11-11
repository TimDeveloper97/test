using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Stage;

namespace NTS.Model.Stage
{
    public class StageSearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal? Time { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public List<string> ListIdSelect { get; set; }
        public string Color { get; set; }
        public bool IsEnable { get;set; }
        public List<StageModel> ListStage { get; set; }

        public StageSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
