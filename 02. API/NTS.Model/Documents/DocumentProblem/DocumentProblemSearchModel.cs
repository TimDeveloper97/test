using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentProblem
{
    public class DocumentProblemSearchModel : SearchCommonModel
    {
        public string DocumentId { get; set; }
        public int? Status { get; set; }
        public string Problem { get; set; }
        public DateTime? ProblemDateFrom { get; set; }
        public DateTime? ProblemDateTo { get; set; }
    }
}
