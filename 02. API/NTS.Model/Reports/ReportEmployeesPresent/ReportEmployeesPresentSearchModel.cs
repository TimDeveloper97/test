using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportEmployeesPresent
{
    public class ReportEmployeesPresentSearchModel : SearchCommonModel
    {
        public DateTime Date { get; set; }
        public string DepartmentId { get; set; }
        public string Code { get; set; }
        public string SBUId { get; set; }
        public string ProjectId { get; set; }
        public string ModuleGroupId { get; set; }
        public int? Year { get; set; }
    }
}
