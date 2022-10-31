using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Applys
{
    public class MoreInterviewsModel
    {
        public string Id { get; set; }
        public string WorkTypeName { get; set; }
        public decimal Salary { get; set; }
        public DateTime ApplyDate { get; set; }
        public int Status { get; set; }
        public int InterviewStatus { get; set; }
        public int ProfileStatus { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string Note { get; set; }
        public string InterviewTime { get; set; }
        public string CandidateApplyId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
    }
}
