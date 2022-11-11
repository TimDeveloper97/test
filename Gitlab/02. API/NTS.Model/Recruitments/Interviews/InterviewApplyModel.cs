using NTS.Model.Candidates;
using NTS.Model.Recruitments.Candidates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Interviews
{
    public class InterviewApplyModel
    {
        public string Id { get; set; }
        public string RecruitmentRequestId { get; set; }
        public string WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public decimal? MinApplySalary { get; set; }
        public decimal? MaxApplySalary { get; set; }
        public string SalaryLevelMinId { get; set; }
        public string SalaryLevelMaxId { get; set; }
        public decimal MinCompanySalary { get; set; }
        public decimal MaxCompanySalary { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public DateTime ApplyDate { get; set; }
        public int Status { get; set; }
        public int InterviewStatus { get; set; }
        public int ProfileStatus { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string StrInterviewTime { get; set; }
        public object InterviewTime { get; set; }
        public string Note { get; set; }
        public string CandidateId { get; set; }
        public InterviewCandidateModel Candidate { get; set; }

        public InterviewApplyModel()
        {
            Candidate = new InterviewCandidateModel();
        }

    }
}
