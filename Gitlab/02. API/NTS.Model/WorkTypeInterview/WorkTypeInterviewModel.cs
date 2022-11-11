using NTS.Model.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkTypeInterview
{
    public class WorkTypeInterviewModel
    {
        public int Id { get; set; }
        public string WorkTypeId { get; set; }
        public string Name { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public List<QuestionSearchResultModel> Questions { get; set; }
        public WorkTypeInterviewModel()
        {
            Questions = new List<QuestionSearchResultModel>();
        }
    }
}
