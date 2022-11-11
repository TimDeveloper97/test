using NTS.Model.Candidates;
using NTS.Model.Recruitments.Candidates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Applys
{
    public class ApplyCreateModel
    {
        public string Id { get; set; }
        public string RecruitmentRequestId { get; set; }
        public string WorkTypeId { get; set; }
        public decimal? MinApplySalary { get; set; }
        public decimal? MaxApplySalary { get; set; }
        public DateTime ApplyDate { get; set; }
        public int Status { get; set; }
        //public int InterviewStatus { get; set; }
        public int ProfileStatus { get; set; }
        //public DateTime? InterviewDate { get; set; }
        public string StrInterviewTime { get; set; }
        //public object InterviewTime { get; set; }
        public string Note { get; set; }
        public string CandidateId { get; set; }
        public bool FollowStatus { get; set; }
        public CandidateCreateModel Candidate { get; set; }

        public ApplyCreateModel()
        {
            Candidate = new CandidateCreateModel();
        }

    }
}
