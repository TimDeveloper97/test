using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Question
{
    public class QuestionModel
    {
        public string Id { get; set; }
        public string QuestionGroupId { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public int Score { get; set; }
        public string Content { get; set; }
        public string Answer { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        public List<QuestionFileModel> QuestionFiles { get; set; }
        public QuestionModel()
        {
            QuestionFiles = new List<QuestionFileModel>();
        }
    }
}
