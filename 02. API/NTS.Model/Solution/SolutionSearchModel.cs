using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Solution
{
    public class SolutionSearchModel: SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string SBUId { get; set; }
        public string CustomerName { get; set; }
        public string EndCustomerName { get; set; }
        public int Status { get; set; }
        public string DepartmentId { get; set; }
        public string SolutionGroupId { get; set; }
        public string SolutionMaker { get; set; }
        public string ProjectId { get; set; }
        public bool IsExport { get; set; }
        public string Typedocuments { get; set; }
        public List<string> ListIdSelect { get; set; }
        public string BusinessDomain { get; set; }


        public SolutionSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
