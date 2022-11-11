using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UserHistory
{
    public class CandidateApplyHistoryModel
    {
        public string Id { get; set; }
        public string CandidateId { get; set; }
        public string WorkTypeId { get; set; }
        public decimal Salary { get; set; }
        public System.DateTime ApplyDate { get; set; }
        public int ProfileStatus { get; set; }
        public int InterviewStatus { get; set; }
        public int Status { get; set; }
        public Nullable<System.DateTime> InterviewDate { get; set; }
        public string Note { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }

    }
}
