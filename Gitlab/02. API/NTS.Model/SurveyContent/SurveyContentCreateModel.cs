using NTS.Model.CustomerRequirementEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SurveyContent
{
    public class SurveyContentCreateModel : BaseModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string Result { get; set; }
        public string SurveyId { get; set; }
        public string Name { get; set; }
        public string EmployeeId { get; set; }
        public List<SurveyContentAttachModel> ListSurveyContentAttach { get; set; }
        public int Level { get; set; }

        //public List<CustomerRequirementUserInfoModel> ListUser { get; set; }
        public SurveyContentCreateModel()
        {
            ListSurveyContentAttach = new List<SurveyContentAttachModel>();
            //ListUser = new List<CustomerRequirementUserInfoModel>();
        }

    }
}
