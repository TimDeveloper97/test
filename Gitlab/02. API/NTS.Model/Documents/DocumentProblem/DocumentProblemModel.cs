using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentProblem
{
    public class DocumentProblemModel
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string Problem { get; set; }
        public int Status { get; set; }
        public System.DateTime ProblemDate { get; set; }
        public Nullable<System.DateTime> FinishExpectedDate { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
