using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Plans
{
    public class WorkingTimeSearchModel : SearchCommonModel
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public string NameCode { get; set; }
        public string WorkType { get; set; }
    }
}
