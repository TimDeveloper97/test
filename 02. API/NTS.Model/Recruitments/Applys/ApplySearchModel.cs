using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Applys
{
    public class ApplySearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkTypeId { get; set; }
        public string RecruitmentRequestId { get; set; }
        public string Email { get; set; }
        public int? ProfileStatus { get; set; }
        public int? Status { get; set; }
        public int? InterviewStatus { get; set; }
        public DateTime? ApplyDateTo { get; set; }
        public DateTime? ApplyDateFrom { get; set; }
        public string NameTraining { get; set; }
        public string LanguagesId { get; set; }
        public string FollowStatus { get; set; }

    }
}
