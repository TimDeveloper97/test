using NTS.Model.QLTKGROUPMODUL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DashboardEmployee
{
    public class DashboardEmployeeSearchModel : SearchCommonModel
    {
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public string WorkType { get; set; }
        public int Type { get; set; }
        public int Year { set; get; }
        public int Quarter { set; get; }
        public int Month { set; get; }
        public string TimeType { set; get; }
        public List<string> ListModuleGroupId { get; set; }
        public DashboardEmployeeSearchModel()
        {
            ListModuleGroupId = new List<string>();
        }
    }
}
