using NTS.Model.Candidates;
using NTS.Model.Recruitments.Candidates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Interviews
{
    public class InterviewModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public string InterviewId { get; set; }
        public string InterviewTime { get; set; }
        public InterviewApplyModel Apply { get; set; }
        public InterviewCandidateModel Candidate { get; set; }
        public List<InterviewQuestionModel> Questions { get; set; }
        public List<InterviewUserInfoModel> ListUser { get; set; }
        public InterviewModel()
        {
            Questions = new List<InterviewQuestionModel>();
            ListUser = new List<InterviewUserInfoModel>();
        }

    }
}
