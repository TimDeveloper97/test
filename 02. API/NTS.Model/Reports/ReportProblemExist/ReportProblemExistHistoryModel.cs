using NTS.Model.ReportProblemExistDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportProblemExist
{
    public class ReportProblemExistHistoryModel
    {
        public string Id { get; set; }
        public string CreateReportBy { get; set; }
        public System.DateTime CreateReportDate { get; set; }
        public string ProjectCode { get; set; }
        public string PRCode { get; set; }
        public string TimeType { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Quarter { get; set; }
        public Nullable<int> Month { get; set; }
        public string EmployeeId { get; set; }
        public string SupplierId { get; set; }
        public List<ReportProblemExistDocumentCreateModel> ListProblem { get; set; }
        public ReportProblemExistHistoryModel()
        {
            ListProblem = new List<ReportProblemExistDocumentCreateModel>();
        }
    }
}
