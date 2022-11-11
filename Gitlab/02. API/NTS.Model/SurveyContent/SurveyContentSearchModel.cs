using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SurveyContent
{
    public class SurveyContentSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Result { get; set; }
        public string SurveyId { get; set; }
        public List<string> ListIdSelect { get; set; }

        public SurveyContentSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
