using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.RecruitmentRequest
{
    public class RecruitmentRequestSearchResultModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public int Quantity { get; set; }
        public DateTime RecruitmentDeadline { get; set; }
        public int Type { get; set;  }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public decimal MinCompanySalary { get; set; }
        public decimal MaxCompanySalary { get; set; }
        public string RecruitmentReason { get; set; }
        public string Description { get; set; }
        public string Request { get; set; }
        public string Equipment { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public int NumberRecruitment { get; set; }
        public int NumberCandidate { get; set; }
        public int NumberInterview { get; set; }
        public int NumberNeedRecruit { get; set; }
        public int NumberLateDate { get; set; }
        public int StatusRecruit { get; set; }
    }
}
