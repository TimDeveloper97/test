using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportProblemExistDocument
{
    public class ReportProblemExistDocumentCreateModel
    {
        public string Id { get; set; }
        public string ReportProblemExistId { get; set; }
        public string Note { get; set; }
        public string Plan { get; set; }
        public string ImportProfileId { get; set; }
        public string SupplierId { get; set; }
        public int Step { get; set; }
        public Nullable<int> LateDayNumber { get; set; }
        public int Status { get; set; }
    }
}
