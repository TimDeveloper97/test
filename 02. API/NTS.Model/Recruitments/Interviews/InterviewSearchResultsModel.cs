using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Interviews
{
    public class InterviewSearchResultsModel
    {
        public string Id { get; set; }
        public string WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public decimal? MinAppySalary { get; set; }
        public decimal? MaxAppySalary { get; set; }
        public DateTime ApplyDate { get; set; }
        public int ProfileStatus { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string EmployeeId { get; set; }
        public List<InterviewModel> Interviews { get; set; }
        public int Count { get; set; }
        public string InterviewBy { get; set; }
        public string MainInterviewer { get; set; }
        public string SubInterviewer { get; set; }

        public string CandidateApplyId { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string InterviewTime { get; set; }
        public int Status { get; set; }
        public List<InterviewUserInfoModel> ListUser { get; set; }
        public List<InterviewQuestionModel> Questions { get; set; }

        public InterviewSearchResultsModel()
        {
            Questions = new List<InterviewQuestionModel>();
            ListUser = new List<InterviewUserInfoModel>();
        }
    }
}
