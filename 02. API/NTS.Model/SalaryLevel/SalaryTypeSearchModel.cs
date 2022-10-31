using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SalaryLevel
{
    public class SalaryTypeSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
