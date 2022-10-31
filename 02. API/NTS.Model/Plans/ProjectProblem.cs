using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class ProjectProblem
    {
        public int RiskNoAction { get; set; }
        public int Done { get; set; }
        public int Implementation { get; set; }
        public int NoImplementation { get; set; }
        public int ErrorDelay { get; set; }
    }
}
