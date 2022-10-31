using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Solution
{
    public class ProtectSolutionContentModel : BaseModel
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string Request { get; set; }
        public string Solution { get; set; }
        public string SolutionId { get; set; }
        public string Note { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
