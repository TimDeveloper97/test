using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Applys
{
    public class ApplySearchResultsModel
    {
        public string Id { get; set; }
        public string WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public decimal? MinApplySalary { get; set; }
        public decimal? MaxApplySalary { get; set; }
        public DateTime ApplyDate { get; set; }
        public int Status { get; set; }
        public int InterviewStatus { get; set; }
        public int ProfileStatus { get; set; }
        //public DateTime? InterviewDate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string RecruitmentRequestId { get; set; }
        public string RecruitmentRequestCode { get; set; }
        public bool FollowStatus { get; set; }
        public string CandidatesId { get; set; }
        public int Total { get; set; }
        public int UnInterview { get; set; }
        public int Pass { get; set; }
        public int Fail { get; set; }
    }
}
