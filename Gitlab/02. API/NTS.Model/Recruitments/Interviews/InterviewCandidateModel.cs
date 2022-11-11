using NTS.Model.Candidates;
using NTS.Model.Recruitments.Candidates;
using NTS.Model.Recruitments.Interviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Interviews
{
    public class InterviewCandidateModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string IdentifyNum { get; set; }
        public DateTime? IdentifyDate { get; set; }
        public string IdentifyAddress { get; set; }
        public int ApplyStatus { get; set; }
        public int InterviewStatus { get; set; }
        public int ProfileStatus { get; set; }
        public bool FollowStatus { get; set; }
        public bool AcquaintanceStatus { get; set; }
        public string AcquaintanceName { get; set; }
        public string DepartmentName { get; set; }
        public string SBUName { get; set; }
        public string RecruitmentChannelId { get; set; }
        public string AcquaintanceNote { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public List<InterviewCandidateEducationModel> Educations { get; set; }
        public List<InterviewCandidateWorkTypeFitModel> WorkTypes { get; set; }
        public List<CandidateLanguageModel> Languages { get; set; }

        public List<InterviewCandidateWorkHistoryModel> WorkHistories { get; set; }
        public List<CandidateAttachModel> Attachs { get; set; }

        public InterviewCandidateModel()
        {
            Educations = new List<InterviewCandidateEducationModel>();
            WorkTypes = new List<InterviewCandidateWorkTypeFitModel>();
            Languages = new List<CandidateLanguageModel>();
            WorkHistories = new List<InterviewCandidateWorkHistoryModel>();
            Attachs = new List<CandidateAttachModel>();
        }
    }
}
