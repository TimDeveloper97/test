using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Candidates
{
    public class CandidateSearchResultsModel
    {
        public string Id { get; set; }
        public string ApplyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int ApplyStatus { get; set; }
        public int InterviewStatus { get; set; }
        public int ProfileStatus { get; set; }
        public bool FlowStatus { get; set; }
        public bool AcquaintanceStatus { get; set; }
        public DateTime? ApplyDate { get; set; }
        public string WorkTypeName { get; set; }
        public string WorkTypeId { get; set; }
        public DateTime? InterviewDate { get; set; }
        public decimal? MinApplySalary { get; set; }
        public decimal? MaxApplySalary { get; set; }
        public string CandidateApplyId { get; set; }
        public string RecruitmentChannelId { get; set; }
    }
}
