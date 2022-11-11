using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Solution
{
    public class CustomerRequirementSolutionContentModel : BaseModel
    {
        public string ID { get; set; }
        public string Content { get; set; }
        public string AnswerTPA { get; set; }
        public string CustomerRequirementSolutionId { get; set; }

    }
}
