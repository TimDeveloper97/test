using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.RecruitmentRequest
{
    public class RecruitmentRequestInfoModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string DepartmentId { get; set; }
        public string WorkTypeId { get; set; }
        public int Quantity { get; set; }
        public DateTime RecruitmentDeadline { get; set; }
        public int Type { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public decimal MinCompanySalary { get; set; }
        public decimal MaxCompanySalary { get; set; }
        public string RecruitmentReason { get; set; }
        public string Description { get; set; }
        public string Request { get; set; }
        public string Equipment { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public int Index { get; set; }
        public int Status { get; set; }
        public string UpdateBy { get; set; }
        public List<RecruitmentRequestAttachModel> ListAttach { get; set; }
        public RecruitmentRequestInfoModel()
        {
            ListAttach = new List<RecruitmentRequestAttachModel>();
        }
    }
}
