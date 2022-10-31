using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Question
{
    public class QuestionSearchResultModel
    {
        public string Id { get; set; }
        public string QuestionGroupId { get; set; }
        public string QuestionGroupName { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public string Answer { get; set; }
        public DateTime? CreateDate { get; set; }

        public List<QuestionFileModel> ListFile { get; set; }
        public QuestionSearchResultModel()
        {
            ListFile = new List<QuestionFileModel>();
        }
    }
}
