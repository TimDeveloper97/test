using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentProblem
{
    public class DocumentProblemSearchResultModel
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public string Problem { get; set; }
        public DateTime ProblemDate { get; set; }
        public DateTime? FinishExpectedDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
