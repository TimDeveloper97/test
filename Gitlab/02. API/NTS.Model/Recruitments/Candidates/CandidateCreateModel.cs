using NTS.Model.Recruitments.Candidates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Candidates
{
    public class CandidateCreateModel
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
        //public string DepartmentId { get; set; }
        //public string SBUId { get; set; }
        public string RecruitmentChannelId { get; set; }
        public string AcquaintanceNote { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public int Index { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public List<CandidateEducationModel> Educations { get; set; }
        public List<CandidateWorkTypeFitModel> WorkTypes { get; set; }
        public List<CandidateLanguageModel> Languages { get; set; }

        public List<CandidateWorkHistoryModel> WorkHistories { get; set; }
        public List<CandidateAttachModel> Attachs { get; set; }
        public string WorkTypeName { get; set; }
        public string IdWorkType { get; set; }
        public string RecruitmentChannelName { get; set; }
        public string CandidateId { get; set; }
        public decimal? MinApplySalary { get; set; }
        public decimal? MaxApplySalary { get; set; }
        public string RecruitmentRequestId { get; set; }

        public CandidateCreateModel()
        {
            Educations = new List<CandidateEducationModel>();
            WorkTypes = new List<CandidateWorkTypeFitModel>();
            Languages = new List<CandidateLanguageModel>();
            WorkHistories = new List<CandidateWorkHistoryModel>();
            Attachs = new List<CandidateAttachModel>();
        }
    }
}
