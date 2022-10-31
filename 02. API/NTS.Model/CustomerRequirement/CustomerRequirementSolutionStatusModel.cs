using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerRequirement
{
    public class CustomerRequirementSolutionStatusModel
    {
        public string CustomerRequirementId { get; set; }
        public string SolutionId { get; set; }
        public int StatusSolution { get; set; }
    }
}
