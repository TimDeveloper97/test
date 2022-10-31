using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SalaryLevel
{
    public class SalaryLevelSearchModel: SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string SalaryGroupId { get; set; }
        public string SalaryTypeId { get; set; }
    }
}
