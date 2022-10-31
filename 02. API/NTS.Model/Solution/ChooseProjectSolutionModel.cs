using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Solution
{
    public class ChooseProjectSolutionModel : SearchCommonModel
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string Customer { get; set; }
        public string SBUName { get; set; }
        public string DepartmentName { get; set; }
        public List<string> ListIdSelect { get; set; }
        public string ProjectProductId { get; set; }

        public ChooseProjectSolutionModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
