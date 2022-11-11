using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Question
{
    public class QuestionSearchModel: SearchCommonModel
    {
        public string QuestionGroupId { get; set; }
        public string Code { get; set; }
        public int? Type { get; set; }

        public List<string> ListIdSelect { get; set; }

        public QuestionSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
