using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GeneralInfomationProject
{
    public class GeneralInfomationProjectSearchModel
    {
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
