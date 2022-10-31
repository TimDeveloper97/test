using NTS.Model.Recruitments.Candidates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Candidates
{
    public class CandidateApplyModel
    {
        public string Id { get; set; }
        public string WorkTypeName { get; set; }
        public decimal? MinApplySalary { get; set; }
        public decimal? MaxApplySalary { get; set; }
        public DateTime ApplyDate { get; set; }
        public int Status { get; set; }
        public int InterviewStatus { get; set; }
        public int ProfileStatus { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string Note { get; set; }
        public string RecruitmentRequestId { get; set; }
        public string CandidateId { get; set; }
        public string WorkTypeId { get; set; }
    }
}
