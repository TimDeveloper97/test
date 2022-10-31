using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectPhase
{
    public class ProjectPhaseSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SBUId { get; set; }
        public string Parentd { get; set; }
    }
}
