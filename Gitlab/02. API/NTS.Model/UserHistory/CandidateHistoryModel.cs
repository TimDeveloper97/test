using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UserHistory
{
    public class CandidateHistoryModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string IdentifyNum { get; set; }
        public Nullable<System.DateTime> IdentifyDate { get; set; }
        public string IdentifyAddress { get; set; }
        public int ApplyStatus { get; set; }
        public int InterviewStatus { get; set; }
        public int ProfileStatus { get; set; }
        public bool FollowStatus { get; set; }
        public bool AcquaintanceStatus { get; set; }
        public string AcquaintanceName { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string AcquaintanceNote { get; set; }
        public string WorkTypeId { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public string RecruitmentChannelId { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
