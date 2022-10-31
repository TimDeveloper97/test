using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskTimeStandardModel
{
    public class CalculateAverageTaskTimeStandardModel
    {
        public string DepartmentId { get; set; }
        public string ModuleGroupId { get; set; }
        public string Name { get; set; }
        public string SBUId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool IsCalculate { get; set; }
        public bool IsExportExcel { get; set; }
    }
}
