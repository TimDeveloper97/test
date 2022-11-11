using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.LaborContract
{
    public class LaborContractSearchModel: SearchCommonModel
    {
        public string Name { get; set; }
        public int? Type { get; set; }
    }
}
