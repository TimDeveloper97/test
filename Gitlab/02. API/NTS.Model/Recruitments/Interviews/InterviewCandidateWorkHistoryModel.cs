using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Interviews
{
    public class InterviewCandidateWorkHistoryModel
    {
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public int TotalTime { get; set; }
        public string WorkTypeName { get; set; }
        public string ReferencePerson { get; set; }
        public string ReferencePersonPhone { get; set; }
        public int? NumberOfManage { get; set; }
        public decimal? Income { get; set; }
        public bool Status { get; set; }
    }
}
