using NTS.Model.CustomerRequirementEmployee;
using NTS.Model.CustomerRequirementMaterial;
using NTS.Model.SurveyContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Survey
{
    public class SurveyInfoModel
    {
        public string Id { get; set; }
        public string ProjectPhaseId { get; set; }
        public string CustomerRequirementId { get; set; }
        public DateTime SurveyDate { get; set; }
        public string Result { get; set; }
        public string Times { get; set; }
        public object Time { get; set; }
        public string Content { get; set; }
        public int Level { get; set; }
        public List<CustomerRequirementUserInfoModel> ListUser { get; set; }
        public List<CustomerRequirementMaterialInfoModel> ListMaterial { get; set; }
        public List<SurveyContentCreateModel> ListRequest { get; set; }
        public string CustomerContactId { get; set; }
        public string Description { get; set; }

        public SurveyInfoModel()
        {

            ListUser = new List<CustomerRequirementUserInfoModel>();
            ListMaterial = new List<CustomerRequirementMaterialInfoModel>();
            ListRequest = new List<SurveyContentCreateModel>();


        }
    }
}
