using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerRequirementEmployee
{
    public class CustomerRequirementUserInfoModel
    {
        public string Id { get; set; }
        public string SurveyId { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public bool IsNew { get; set; }
        public string SurveyContentId { get; set; }

    }
}
