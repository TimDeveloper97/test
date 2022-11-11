using NTS.Model.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectGeneralDesign
{
    public class ProjectGaneralDesignApproveStatusModel
    {
        public string Id { get; set; }
        public int ApproveStatus { get; set; }
        public List<PlanResultModel> ListPlan { get; set; }
        public ProjectGaneralDesignApproveStatusModel()
        {
            ListPlan = new List<PlanResultModel>();
        }
    }
}
