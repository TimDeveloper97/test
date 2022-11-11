using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Survey
{
    public class SurveySearchResultModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProjectPhaseId { get; set; }
        public string CustomerRequirementId { get; set; }
        public DateTime SurveyDate { get; set; }
        public string Times { get; set; }
        public object Time { get; set; }
        public int Level { get; set; }
        public List<string> ListSurveyId { get; set; }
        public SurveySearchResultModel()
        {
            ListSurveyId = new List<string>();
        }

    }
}
